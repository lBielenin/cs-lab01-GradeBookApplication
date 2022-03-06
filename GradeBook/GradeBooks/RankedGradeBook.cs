using System;
using System.Collections.Generic;
using System.Linq;

namespace GradeBook.GradeBooks
{
    public class RankedGradeBook : BaseGradeBook
    {
        public RankedGradeBook(string name) : base(name)
        {
            Type = Enums.GradeBookType.Ranked;
        }

        public override char GetLetterGrade(double averageGrade)
        {
            if (Students.Count < 5)
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

        private List<List<Student>> ChunkEvenly(List<Student> list)
        {
            int count = list.Count;
            IOrderedEnumerable<Student> ordByGrade = list.OrderByDescending(s => s.AverageGrade);
            int chunkSize = count / Grades.Count;

            return ordByGrade
                .Select((val, i) => new { idx = i, Value = val })
                .GroupBy(obj => obj.idx / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
