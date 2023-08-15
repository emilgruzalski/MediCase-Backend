using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCase.WebAPI.Entities.Moderator;
using MediCase.WebAPI.Repositories.Moderator.Interfaces;
using MediCase.WebAPI.Exceptions;

namespace MediCase.WebAPI.Repositories.Moderator
{
    public class EntityTranslationFilesRepository : IEntityTranslationFilesRepository
    {
        private readonly MediCaseModeratorContext _context;
        private readonly ILogger _logger;

        public EntityTranslationFilesRepository(MediCaseModeratorContext context, ILogger<EntityTranslationFilesRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<EntityTranslationFile> AddEntityTranslationFileAsync(EntityTranslationFile newEntityTranslationFile)
        {
            _context.EntityTranslationFiles.Add(newEntityTranslationFile);
            await _context.SaveChangesAsync();
            _context.Entry(newEntityTranslationFile).State = EntityState.Detached;
            return newEntityTranslationFile;
        }
        public async Task DeleteEntityTranslationFileAsync(uint entityTranslationFileId)
        {
            var entityTranslationFile = _context.EntityTranslationFiles.Where(x => x.FileId == entityTranslationFileId).FirstOrDefault();
            if (entityTranslationFile == null) throw new NotFoundException($"File not found: {entityTranslationFileId}");
            _context.Remove(entityTranslationFile);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateEntityTranslationFileAsync(EntityTranslationFile entityTranslationFile)
        {
            _context.Update(entityTranslationFile);
            await _context.SaveChangesAsync();
        }
        public async Task<uint> GetNextFileIdAsync()
        {
            if (await _context.EntityTranslationFiles.AnyAsync())
            {
                return await _context.EntityTranslationFiles.MaxAsync(x => x.FileId) + 1;
            }

            return 1;

        }

        public async Task<string> GetActualFilePathAsync(string hashedFileName)
        {
            var file = await _context.EntityTranslationFiles.Where(x => x.FilePathHashed == hashedFileName).FirstOrDefaultAsync();
            if (file == null) throw new NotFoundException("File not found");
            return file.FilePath;
        }

        public async Task<string> GetActualFilePathAsync(uint fileId)
        {
            var file = await _context.EntityTranslationFiles.Where(x => x.FileId == fileId).FirstOrDefaultAsync();
            if (file == null) throw new NotFoundException($"File not found: {fileId}");
            return file.FilePath;
        }
    }
}
