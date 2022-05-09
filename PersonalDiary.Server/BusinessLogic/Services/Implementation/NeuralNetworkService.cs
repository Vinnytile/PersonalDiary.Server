using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using BusinessLogic.Services.Helpers;
using Emgu.CV.Face;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SharedData.Models;
using Emgu.CV.CvEnum;
using static Emgu.CV.Face.FaceRecognizer;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Services
{
    public class NeuralNetworkService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private string facePath;
        private string savePath;
        private CascadeClassifier classifierFace;
        private EigenFaceRecognizer recognizer;

        private DateTime lastSavingTime = DateTime.Now;

        private Dictionary<string, List<Image<Gray, Byte>>> usersFaces = new Dictionary<string, List<Image<Gray, byte>>>();
        public Dictionary<string, bool> usersRegisterSucceed = new Dictionary<string, bool>();
        public Dictionary<string, bool> usersLoginSucceed = new Dictionary<string, bool>();
        List<User> users;

        private int numImagesPerUser = 20;

        public NeuralNetworkService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            facePath = Path.GetFullPath(@"../BusinessLogic/Services/Data/haarcascade_frontalface_default.xml");
            savePath = Path.GetFullPath(@"../BusinessLogic/Services/Data/");

            classifierFace = new CascadeClassifier(facePath);
            recognizer = new EigenFaceRecognizer(0, 10000);

            LoadRecongizer();
        }

        private Image<Bgr, Byte> LoginFace(Image<Bgr, Byte> imageIn, Guid userId)
        {
            if (users == null)
            {
                LoadUsersAsync();
            }

            var imgGray = imageIn.Convert<Gray, byte>().Clone();
            Rectangle[] faces = classifierFace.DetectMultiScale(imgGray, 1.1, 4);

            bool isUserSucceed = usersLoginSucceed.TryGetValue(userId.ToString(), out bool userSucceed);
            if (!isUserSucceed)
            {
                userSucceed = false;
                usersLoginSucceed.Add(userId.ToString(), userSucceed);
            }

            var user = users.FirstOrDefault(u => u.Id == userId);

            foreach (var face in faces)
            {
                imageIn.Draw(face, new Bgr(0, 0, 255), 2);
                var testImage = imgGray.Copy(face).Resize(100, 100, Inter.Cubic);
                PredictionResult result = recognizer.Predict(testImage);
                var faceId = result.Label;

                bool isUserLogined = (faceId == user.FaceId);

                if (!isUserLogined)
                {
                    imageIn.Draw("unknown", new Point(face.X - 2, face.Y - 2), FontFace.HersheySimplex, 2.0, new Bgr(Color.Black));
                }

                if (isUserLogined)
                {
                    imageIn.Draw(user.Username, new Point(face.X - 2, face.Y - 2), FontFace.HersheySimplex, 2.0, new Bgr(Color.Black));
                    usersLoginSucceed[userId.ToString()] = true;
                }
            }

            return imageIn;
        }

        private Image<Bgr, Byte> RegisterFace(Image<Bgr, Byte> imageIn, Guid userId, bool shouldSave)
        {
            var imgGray = imageIn.Convert<Gray, byte>().Clone();
            Rectangle[] faces = classifierFace.DetectMultiScale(imgGray, 1.1, 4);

            bool isUserExist = usersFaces.TryGetValue(userId.ToString(), out List<Image<Gray, Byte>> userFaces);
            if (!isUserExist)
            {
                userFaces = new List<Image<Gray, byte>>();
                usersFaces.Add(userId.ToString(), userFaces);
            }
            bool isUserSucceed = usersRegisterSucceed.TryGetValue(userId.ToString(), out bool userSucceed);
            if (!isUserSucceed)
            {
                userSucceed = false;
                usersRegisterSucceed.Add(userId.ToString(), userSucceed);
            }

            foreach (var face in faces)
            {
                imageIn.Draw(face, new Bgr(0, 0, 255), 2);

                bool isUserRegistered = userFaces.Count == numImagesPerUser;
                
                if (!isUserRegistered && shouldSave)
                {
                    DateTime currentSavingTime = DateTime.Now;
                    if (currentSavingTime.Subtract(lastSavingTime) > TimeSpan.FromSeconds(0.5))
                    {
                        Image<Gray, Byte> result = imgGray.Copy(face).Resize(100, 100, Inter.Cubic);
                        userFaces.Add(result);
                        lastSavingTime = currentSavingTime;
                    }
                }

                if (isUserRegistered)
                {
                    usersRegisterSucceed[userId.ToString()] = true;
                }
            }

            return imageIn;
        }

        public async Task TrainFaceAsync(Guid userId)
        {
            User user;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var myScopedService = scope.ServiceProvider.GetService<DataContext>();

                user = await myScopedService.Users.FirstOrDefaultAsync(u => u.Id == userId);
            }
            int faceId = user.FaceId;

            List<Mat> matList = new List<Mat>();
            List<int> intList = new List<int>();
            usersFaces.TryGetValue(userId.ToString(), out List<Image<Gray, Byte>> imagesList);
            foreach (var image in imagesList)
            {
                matList.Add(image.Mat);
                intList.Add(faceId);
            }

            recognizer.Train(matList.ToArray(), intList.ToArray());

            SaveRecognizer();
 
            ClearRegisterData(userId.ToString());
        }

        private void LoadRecongizer()
        {
            bool isFileExist = File.Exists(savePath + "recognizer.json");
            
            if (isFileExist)
            {
                recognizer.Read(savePath + "recognizer.json");
            }
        }

        private void SaveRecognizer()
        {
            recognizer.Write(savePath + "recognizer.json");
        }

        public async Task LoadUsersAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var myScopedService = scope.ServiceProvider.GetService<DataContext>();

                users = await myScopedService.Users.ToListAsync();
            }
        }

        public void ClearRegisterData(string userId)
        {
            usersFaces.Remove(userId);
            usersRegisterSucceed.Remove(userId);
        }

        public void ClearLoginData(string userId)
        {
            usersLoginSucceed.Remove(userId);
        }

        public string ProcessLoginFace(string content, Guid userId)
        {
            Image image = ImageHelper.Base64ToImage(content);
            Image<Bgr, Byte> emguCVImage = ImageHelper.ImageToEmguCVImage(image);
            Image<Bgr, Byte> resultEmguCVImage = LoginFace(emguCVImage, userId);
            Image resultImage = ImageHelper.EmguCVImagetoImage(resultEmguCVImage);
            string base64 = ImageHelper.ImageToBase64(resultImage, ImageFormat.Jpeg);

            return base64;
        }

        public string ProcessRegisterFace(string content, Guid userId, bool shouldSave)
        {
            Image image = ImageHelper.Base64ToImage(content);
            Image<Bgr, Byte> emguCVImage = ImageHelper.ImageToEmguCVImage(image);
            Image<Bgr, Byte> resultEmguCVImage = RegisterFace(emguCVImage, userId, shouldSave);
            Image resultImage = ImageHelper.EmguCVImagetoImage(resultEmguCVImage);
            string base64 = ImageHelper.ImageToBase64(resultImage, ImageFormat.Jpeg);

            return base64;
        }
    }
}
