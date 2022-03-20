using System;
using System.Collections.Generic;
using System.Linq;

namespace GradeBook.GradeBooks
{
    public class RankedGradeBook : BaseGradeBook
    {
        public RankedGradeBook(string name, bool isWeight) : base(name, isWeight)
        {
            Type = Enums.GradeBookType.Ranked;
        }

        public override char GetLetterGrade(double averageGrade)
        {
            if (!CheckIfStudentCountIsSufficent())
                throw new InvalidOperationException();

            IOrderedEnumerable<Student> studentsDividedByGrade = 
                Students.OrderByDescending(s => s.AverageGrade);

            List<List<Student>> chunkedList = ChunkEvenly(Students);

            for (int i = 0; i < chunkedList.Count; i++)
            {
                List<Student> chunk = chunkedList[i];
                if(averageGrade >= chunk.Last().AverageGrade)
                {
                    return Grades[i];
                } 
            }

            return 'F';
        }

        public override  void CalculateStatistics()
        {
            if (!CheckIfStudentCountIsSufficent())
                Console.WriteLine("Ranked grading requires at least 5 students.");
            else
                base.CalculateStatistics();
        }

        public override void CalculateStudentStatistics(string name)
        {
            if (!CheckIfStudentCountIsSufficent())
                Console.WriteLine("Ranked grading requires at least 5 students.");
            else
                base.CalculateStudentStatistics(name);
        }

        private bool CheckIfStudentCountIsSufficent() => Students.Count >= 5;

        private List<List<Student>> ChunkEvenly(List<Student> list)
        {
            int count = list.Count;
            IOrderedEnumerable<Student> ordByGrade = list.OrderByDescending(s => s.AverageGrade);
            int chunkSize = count / Grades.Count;

            return ordByGrade
                .Select((val, i) => new { chunkNum = i / chunkSize, Value = val })
                .GroupBy(obj => obj.chunkNum)
                .Select(group => group.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
