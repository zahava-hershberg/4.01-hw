using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4._01_Hw.Data
{
    public class QuestionRepository
    {
        private readonly string _connectionString;
        public QuestionRepository(string connectionString)
        {
            _connectionString = connectionString;

        }
        private Tag GetTag(string name)
        {
            using var context = new QAContext(_connectionString);
            return context.Tags.FirstOrDefault(t => t.Name == name);
        }
        private int AddTag(string name)
        {
            using var context = new QAContext(_connectionString);
            var tag = new Tag { Name = name };
            context.Tags.Add(tag);
            context.SaveChanges();
            return tag.Id;
        }
      
        public List<Question> GetQuestions()
        {
            using var context = new QAContext(_connectionString);
            return context.Questions.Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .Include(q => q.User)
                .Include(q => q.Answers)
                .ThenInclude(a => a.User)
                .ToList();
        }
        public void AddQuestion(Question question, List<string> tags)
        {
            using var context = new QAContext(_connectionString);
            context.Questions.Add(question);
            context.SaveChanges();
            foreach (string tag in tags)
            {
                Tag t = GetTag(tag);
                int tagId;
                if (t == null)
                {
                    tagId = AddTag(tag);
                }
                else
                {
                    tagId = t.Id;
                }
                context.QuestionsTags.Add(new QuestionTags
                {
                    QuestionId = question.Id,
                    TagId = tagId
                });
            }

            context.SaveChanges();
        }
        public Question GetQuestion(int id)
        {
            using var context = new QAContext(_connectionString);
            return context.Questions.Include(q => q.QuestionTags)
                .ThenInclude(qt => qt.Tag)
                .Include(q => q.User)
                .Include(q => q.Answers)
                .ThenInclude(a => a.User)
                .FirstOrDefault(q => q.Id == id);
        }
        public void AddAnswer(Answer answer)
        {
            using var context = new QAContext(_connectionString);
            context.Answer.Add(answer);
            context.SaveChanges();

        }

    }
}
