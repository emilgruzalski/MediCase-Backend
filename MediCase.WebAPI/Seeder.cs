using MediCase.WebAPI.Entities.Admin;
using MediCase.WebAPI.Entities.Content;
using MediCase.WebAPI.Entities.Moderator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing.Printing;

namespace MediCase.WebAPI
{
    public class Seeder
    {
        private readonly MediCaseAdminContext _dbAdminContext;
        private readonly MediCaseModeratorContext _dbModeratorContext;
        private readonly MediCaseContentContext _dbContentContext;

        public Seeder(MediCaseAdminContext dbAdminContext, MediCaseModeratorContext dbModeratorContext, MediCaseContentContext dbContentContext)
        {
            _dbAdminContext = dbAdminContext;
            _dbModeratorContext = dbModeratorContext;
            _dbContentContext = dbContentContext;
        }

        public void Seed()
        {
            _dbAdminContext.Database.EnsureCreated();
            _dbContentContext.Database.EnsureCreated();
            _dbModeratorContext.Database.EnsureCreated();

            if (!_dbContentContext.EntityTypes.Any()) 
            {
                _dbContentContext.EntityTypes.AddRange(
                    new Entities.Content.EntityType { TypeId = 4, TypeValue = "content" },
                    new Entities.Content.EntityType { TypeId = 6, TypeValue = "content_container" },
                    new Entities.Content.EntityType { TypeId = 11, TypeValue = "glossary" },
                    new Entities.Content.EntityType { TypeId = 12, TypeValue = "glossary_entry" },
                    new Entities.Content.EntityType { TypeId = 3, TypeValue = "navigation" },
                    new Entities.Content.EntityType { TypeId = 5, TypeValue = "question" },
                    new Entities.Content.EntityType { TypeId = 15, TypeValue = "question_container" },
                    new Entities.Content.EntityType { TypeId = 13, TypeValue = "test" },
                    new Entities.Content.EntityType { TypeId = 14, TypeValue = "test_question" }
                );

                _dbContentContext.SaveChanges();
            }

            if (!_dbModeratorContext.EntityTypes.Any()) 
            {
                _dbModeratorContext.EntityTypes.AddRange(
                    new Entities.Moderator.EntityType { TypeId = 4, TypeValue = "content" },
                    new Entities.Moderator.EntityType { TypeId = 6, TypeValue = "content_container" },
                    new Entities.Moderator.EntityType { TypeId = 11, TypeValue = "glossary" },
                    new Entities.Moderator.EntityType { TypeId = 12, TypeValue = "glossary_entry" },
                    new Entities.Moderator.EntityType { TypeId = 3, TypeValue = "navigation" },
                    new Entities.Moderator.EntityType { TypeId = 5, TypeValue = "question" },
                    new Entities.Moderator.EntityType { TypeId = 15, TypeValue = "question_container" },
                    new Entities.Moderator.EntityType { TypeId = 13, TypeValue = "test" },
                    new Entities.Moderator.EntityType { TypeId = 14, TypeValue = "test_question" }
                );

                _dbModeratorContext.SaveChanges();
            }

            if (!_dbContentContext.EntityLanguages.Any())
            {
                _dbContentContext.EntityLanguages.AddRange(
                    new Entities.Content.EntityLanguage { LangId = 1, LangValue = "PL" },
                    new Entities.Content.EntityLanguage { LangId = 2, LangValue = "US" }
                );

                _dbContentContext.SaveChanges();
            }

            if (!_dbModeratorContext.EntityLanguages.Any())
            {
                _dbModeratorContext.EntityLanguages.AddRange(
                    new Entities.Moderator.EntityLanguage { LangId = 1, LangValue = "PL" },
                    new Entities.Moderator.EntityLanguage { LangId = 2, LangValue = "US" }
                );

                _dbModeratorContext.SaveChanges();
            }

            if (!_dbContentContext.Entities.Any()) 
            {
                _dbContentContext.Entities.Add(new Entities.Content.Entity { EntityId = 3, HasChilds = false, LockExpirationDate = DateTime.Now, TypeId = 3, EntityOrder = 0 });

                _dbContentContext.SaveChanges();
            }

            if (!_dbModeratorContext.Entities.Any()) 
            {
                _dbModeratorContext.Entities.Add(new Entities.Moderator.Entity { EntityId = 3, HasChilds = false, LockExpirationDate = DateTime.Now, TypeId = 3, EntityOrder = 0 });

                _dbModeratorContext.SaveChanges();
            }

            if (!_dbContentContext.EntityTranslations.Any()) 
            {
                _dbContentContext.EntityTranslations.AddRange(
                    new Entities.Content.EntityTranslation { EntityId = 3, LangId = 1, MainTitle = "Sections" },
                    new Entities.Content.EntityTranslation { EntityId = 3, LangId = 2, MainTitle = "Sekcje" }
                );

                _dbContentContext.SaveChanges();
            }

            if (!_dbModeratorContext.EntityTranslations.Any())
            {
                _dbModeratorContext.EntityTranslations.AddRange(
                    new Entities.Moderator.EntityTranslation { EntityId = 3, LangId = 1, MainTitle = "Sections" },
                    new Entities.Moderator.EntityTranslation { EntityId = 3, LangId = 2, MainTitle = "Sekcje" }
                );

                _dbModeratorContext.SaveChanges();
            }

            if (!_dbAdminContext.Groups.Any() && !_dbAdminContext.Users.Any()) 
            {
                User John = new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "johndoe@mail.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("foobar") };
                _dbAdminContext.Users.Add(John);
                _dbAdminContext.Groups.Add(new Group { Id = 1, IsAdmin = true, IsModerator = true, IsUser = true, Name = "Full Access" });
                _dbAdminContext.Groups.First().Users.Add(John);
             
                _dbAdminContext.SaveChanges();
            }

            bool DoesFunctionExist(string databaseSchema) 
            {
                System.FormattableString sql = $@"
                    SELECT COUNT(*)
                    FROM information_schema.routines
                    WHERE routine_name = 'DeleteNode'
                    AND routine_type = 'PROCEDURE'
                    AND routine_schema = '{databaseSchema}';
                ";

                int count = 0;

                if (databaseSchema == "medicase_content") 
                {
                    count = _dbContentContext.Database.SqlQuery<int>(sql).FirstOrDefault();
                } else if (databaseSchema == "medicase_moderator") 
                {
                    count = _dbModeratorContext.Database.SqlQuery<int>(sql).FirstOrDefault();
                }

                return count > 0; 
            }

            if (!DoesFunctionExist("medicase_content")) 
            {
                string sql = @"
                    CREATE PROCEDURE `DeleteNode`(
	                    IN `IEntityId` BIGINT
                    )
                    BEGIN
                        CREATE TEMPORARY TABLE ToErase AS
                        WITH RECURSIVE Tree AS 
                        (
		                    (
			                    SELECT a.EdgeId, a.ParentId, a.ChildId, 0 AS depth
			                    FROM EntitiesGraphData a
			                    WHERE a.ParentId=IEntityId OR a.ChildId=IEntityId
		                    )
                            UNION ALL
		                    (
			                    SELECT c.EdgeId, c.ParentId, c.ChildId, b.depth+1
			                    FROM Tree b JOIN EntitiesGraphData c ON (b.ChildId=c.ParentId)
		                    )
	                    )
                        SELECT DISTINCT EdgeId, ChildId FROM Tree;
                        DELETE FROM EntitiesGraphData WHERE EdgeId IN (SELECT EdgeId FROM ToErase);
                        DELETE FROM Entities WHERE EntityId IN (SELECT ChildId FROM ToErase);
                        DROP TABLE ToErase;
                    END;
                ";

                _dbContentContext.Database.ExecuteSqlRaw(sql);
            }

            if (!DoesFunctionExist("medicase_moderator"))
            {
                string sql = @"
                    CREATE PROCEDURE `DeleteNode`(
	                    IN `IEntityId` BIGINT
                    )
                    BEGIN
                        CREATE TEMPORARY TABLE ToErase AS
                        WITH RECURSIVE Tree AS 
                        (
		                    (
			                    SELECT a.EdgeId, a.ParentId, a.ChildId, 0 AS depth
			                    FROM EntitiesGraphData a
			                    WHERE a.ParentId=IEntityId OR a.ChildId=IEntityId
		                    )
                            UNION ALL
		                    (
			                    SELECT c.EdgeId, c.ParentId, c.ChildId, b.depth+1
			                    FROM Tree b JOIN EntitiesGraphData c ON (b.ChildId=c.ParentId)
		                    )
	                    )
                        SELECT DISTINCT EdgeId, ChildId FROM Tree;
                        DELETE FROM EntitiesGraphData WHERE EdgeId IN (SELECT EdgeId FROM ToErase);
                        DELETE FROM Entities WHERE EntityId IN (SELECT ChildId FROM ToErase);
                        DROP TABLE ToErase;
                    END;
                ";

                _dbModeratorContext.Database.ExecuteSqlRaw(sql);
            }
        }
    }
}
