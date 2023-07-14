using Microsoft.EntityFrameworkCore;
using CoursesAndStudents;

using static System.Console;

using (Academy a = new()) {
  bool deleted = await a.Database.EnsureDeletedAsync();
  WriteLine($"Databse deleted: {deleted}");
  bool created = await a.Database.EnsureCreatedAsync();
  WriteLine($"Databse created: {created}");

  WriteLine("SQL script used to create db: ");

  foreach (var s in a.Students.Include(s => s.Courses)) {
    WriteLine("{0} {1} attends the following {2} courses:",
      s.FirstName, s.LastName, s.Courses.Count);
    foreach (var c in s.Courses) {
      WriteLine($"  {c.Title}");
    }
  }
}