namespace RagOpenAiTestApp.Database;

public class Database
{
    public List<Folder> Folders { get; set; } = new();
    public List<Note> Notes { get; set; } = new();

    public Database()
    {
        // Folder 1: Computer Science
        var csFolder = new Folder
        {
            Id = Guid.NewGuid(),
            Name = "Computer Science",
            Description = "Notes related to computer science concepts",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var csNotes = new List<Note>
        {
            new Note
            {
                Id = Guid.NewGuid(),
                Title = "Data Structures",
                Content = "Study of arrays, linked lists, stacks, queues, trees, and graphs.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                FolderId = csFolder.Id,
                Folder = csFolder
            },
            new Note
            {
                Id = Guid.NewGuid(),
                Title = "Algorithms",
                Content = "Includes sorting, searching, dynamic programming, and greedy techniques.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                FolderId = csFolder.Id,
                Folder = csFolder
            },
            new Note
            {
                Id = Guid.NewGuid(),
                Title = "Operating Systems",
                Content = "Concepts such as memory management, processes, and file systems.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                FolderId = csFolder.Id,
                Folder = csFolder
            }
        };

        csFolder.Notes.AddRange(csNotes);

        // Folder 2: Medical Technology
        var medFolder = new Folder
        {
            Id = Guid.NewGuid(),
            Name = "Medical Technology",
            Description = "Notes on recent advances and technologies in medicine",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var medNotes = new List<Note>
        {
            new Note
            {
                Id = Guid.NewGuid(),
                Title = "MRI Scanners",
                Content = "Magnetic Resonance Imaging techniques and use cases.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                FolderId = medFolder.Id,
                Folder = medFolder
            },
            new Note
            {
                Id = Guid.NewGuid(),
                Title = "Robotic Surgery",
                Content = "Applications and benefits of robotics in surgery.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                FolderId = medFolder.Id,
                Folder = medFolder
            },
            new Note
            {
                Id = Guid.NewGuid(),
                Title = "Telemedicine",
                Content = "Remote diagnosis and treatment using telecommunications.",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                FolderId = medFolder.Id,
                Folder = medFolder
            }
        };

        medFolder.Notes.AddRange(medNotes);

        // Finalize database
        Folders.Add(csFolder);
        Folders.Add(medFolder);

        Notes.AddRange(csNotes);
        Notes.AddRange(medNotes);
    }
}



public class Note
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public Guid FolderId { get; set; }
    public Folder Folder { get; set; } = null!;
}

public class Folder
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public List<Note> Notes { get; set; } = new();
}