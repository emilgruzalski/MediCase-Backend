using MediCase.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Runtime.CompilerServices;
using MediCase.WebAPI.Repositories.Moderator.Interfaces;

namespace MediCase.WebAPI.Services
{
    public class FileService : IFileService
    {

        private readonly IEntityTranslationFilesRepository _entityTranslationFilesRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private static readonly string[] _validImageExtensions = { ".jpg", ".bmp", ".gif", ".png" };
        private static readonly string[] _validVoiceExtensions = { ".acc", ".wma", ".wav", ".mp3", ".flac", ".m4a" };
        private static readonly string[] _validVideoExtensions = { ".mp4" };
        public FileService(
            ILogger<FileService> logger,
            IEntityTranslationFilesRepository entityTranslationFilesRepository,
            IConfiguration configuration)
        {

            _entityTranslationFilesRepository = entityTranslationFilesRepository;
            _configuration = configuration;
            Directory.CreateDirectory(Path.Combine(_configuration["PrivateFiles"], "images"));
            Directory.CreateDirectory(Path.Combine(_configuration["PrivateFiles"], "voices"));
            Directory.CreateDirectory(Path.Combine(_configuration["PrivateFiles"], "videos"));

            _logger = logger;
        }

        private string GenerateRandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<Tuple<string, string, string>> SaveFileAsync(IFormFile newFile)
        {
            string filePath = string.Empty;
            string desiredDirectory = string.Empty;
            if (newFile.Length > 0)
            {
                var nextFileId = await _entityTranslationFilesRepository.GetNextFileIdAsync();
                var fileExtension = Path.GetExtension(newFile.FileName);
                desiredDirectory = string.Empty;
                if (_validImageExtensions.Contains(fileExtension)) desiredDirectory = "images";
                if (_validVoiceExtensions.Contains(fileExtension)) desiredDirectory = "voices";
                if (_validVideoExtensions.Contains(fileExtension)) desiredDirectory = "videos";
                if (desiredDirectory == string.Empty) throw new ArgumentException();
                filePath = Path.Combine(desiredDirectory, GenerateRandomString(10) + nextFileId.ToString()) + fileExtension;

                using (var stream = File.Create(Path.Combine(_configuration["PrivateFiles"], filePath)))
                {
                    await newFile.CopyToAsync(stream);
                }
            }
            else
            {
                throw new ArgumentException();
            }
            
            return Tuple.Create(filePath, desiredDirectory.Remove(desiredDirectory.Length - 1, 1),
                BCrypt.Net.BCrypt.HashPassword(filePath));
        }

        public async Task<FileStream> GetFileStreamAsync(string fileName)
        {
            return await Task.Run(() => new FileStream(Path.Combine(_configuration["PrivateFiles"], fileName), FileMode.Open));
        }
        public async Task DeleteFileAsync(string fileName)
        {
            string filePath = Path.Combine(_configuration["PrivateFiles"], fileName);
            if (File.Exists(filePath)) await Task.Run(() => File.Delete(filePath));
        }
    }
}
