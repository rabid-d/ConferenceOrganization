using DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ConferenceDatabaseContext : DbContext
    {
        public virtual DbSet<Conference> Conferences { get; set; } = null!;
        public virtual DbSet<Section> Sections { get; set; } = null!;
        public virtual DbSet<Talk> Talks { get; set; } = null!;
        public virtual DbSet<Equipment> Equipment { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        public ConferenceDatabaseContext()
        {
            if (Database.EnsureCreated())
            {
                FillWithData();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer("Server=localhost;Database=ConferenceDatabase;Integrated Security=SSPI;");
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.HasDefaultSchema("dbo");

            mb.Entity<Conference>().ToTable("Conference").HasKey(c => c.ConferenceId).HasName("Pk_Conference");
            mb.Entity<Conference>().Property(c => c.ConferenceId).HasDefaultValueSql("NEWID()").IsRequired();
            mb.Entity<Conference>().Property(c => c.Name).HasMaxLength(128).IsRequired();
            mb.Entity<Conference>().Property(c => c.Address).HasMaxLength(256).IsRequired();
            mb.Entity<Conference>().Property(c => c.DateStart).IsRequired();
            mb.Entity<Conference>().Property(c => c.DateEnd).IsRequired();

            mb.Entity<Section>().ToTable("Section").HasKey(s => s.SectionId).HasName("Pk_Section");
            mb.Entity<Section>().Property(s => s.SectionId).HasDefaultValueSql("NEWID()").IsRequired();
            mb.Entity<Section>().Property(s => s.Name).HasMaxLength(128).IsRequired();
            mb.Entity<Section>().Property(s => s.ChairpersonId).IsRequired();
            mb.Entity<Section>()
                .HasOne(s => s.Chairperson)
                .WithMany(u => u.Sections)
                .HasForeignKey(s => s.ChairpersonId)
                .HasConstraintName("Fk_Section_User");
            mb.Entity<Section>().Property(s => s.Room).HasMaxLength(32).IsRequired();
            mb.Entity<Section>().Property(s => s.ConferenceId).IsRequired();
            mb.Entity<Section>()
                .HasOne(s => s.Conference)
                .WithMany(c => c.Sections)
                .HasForeignKey(s => s.ConferenceId)
                .HasConstraintName("Fk_Section_Conference");

            mb.Entity<Talk>().ToTable("Talk").HasKey(t => t.TalkId).HasName("Pk_Talk");
            mb.Entity<Talk>().Property(t => t.TalkId).HasDefaultValueSql("NEWID()").IsRequired();
            mb.Entity<Talk>().Property(t => t.Theme).HasMaxLength(128).IsRequired();
            mb.Entity<Talk>().Property(t => t.SpeakerId).IsRequired();
            mb.Entity<Talk>()
                .HasOne(t => t.Speaker)
                .WithMany(u => u.Talks)
                .HasForeignKey(t => t.SpeakerId)
                .HasConstraintName("Fk_Talk_User");
            mb.Entity<Talk>().Property(t => t.DateStart).IsRequired();
            mb.Entity<Talk>().Property(t => t.DateEnd).IsRequired();
            mb.Entity<Talk>()
                .HasOne(t => t.Section)
                .WithMany(s => s.Talks)
                .HasForeignKey(t => t.SectionId)
                .HasConstraintName("Fk_Talk_Section")
                .OnDelete(DeleteBehavior.Restrict);
            mb.Entity<Talk>()
                .HasMany(t => t.Equipment)
                .WithMany(e => e.Talks);

            mb.Entity<Equipment>().ToTable("Equipment").HasKey(e => e.EquipmentId).HasName("Pk_Equipment");
            mb.Entity<Equipment>().Property(e => e.EquipmentId).HasDefaultValueSql("NEWID()").IsRequired();
            mb.Entity<Equipment>().Property(e => e.Name).HasMaxLength(128).IsRequired();

            mb.Entity<User>().ToTable("User").HasKey(u => u.UserId).HasName("Pk_User");
            mb.Entity<User>().Property(u => u.UserId).HasDefaultValueSql("NEWID()").IsRequired();
            mb.Entity<User>().Property(u => u.FullName).HasMaxLength(128).IsRequired();
            mb.Entity<User>().Property(u => u.Degree).HasMaxLength(128).IsRequired();
            mb.Entity<User>().Property(u => u.Work).HasMaxLength(128).IsRequired();
            mb.Entity<User>().Property(u => u.Position).HasMaxLength(128).IsRequired();
            mb.Entity<User>().Property(u => u.ProfessionalBiography).IsRequired();
            mb.Entity<User>().Property(u => u.PathToPhoto).IsRequired(false);



            mb.Entity<Conference>().Property(c => c.ModifiedBy).IsRequired(false);
            mb.Entity<Conference>().Property(c => c.ModifiedDate).IsRequired(false);
            mb.Entity<Conference>().Property(c => c.CreatedBy).HasDefaultValueSql("NEWID()").IsRequired();
            mb.Entity<Conference>().Property(c => c.CreatedDate).HasDefaultValueSql("GETUTCDATE()").IsRequired();
            mb.Entity<Section>().Property(c => c.ModifiedBy).IsRequired(false);
            mb.Entity<Section>().Property(c => c.ModifiedDate).IsRequired(false);
            mb.Entity<Section>().Property(c => c.CreatedBy).HasDefaultValueSql("NEWID()").IsRequired();
            mb.Entity<Section>().Property(c => c.CreatedDate).HasDefaultValueSql("GETUTCDATE()").IsRequired();
            mb.Entity<Talk>().Property(c => c.ModifiedBy).IsRequired(false);
            mb.Entity<Talk>().Property(c => c.ModifiedDate).IsRequired(false);
            mb.Entity<Talk>().Property(c => c.CreatedBy).HasDefaultValueSql("NEWID()").IsRequired();
            mb.Entity<Talk>().Property(c => c.CreatedDate).HasDefaultValueSql("GETUTCDATE()").IsRequired();
            mb.Entity<Equipment>().Property(c => c.ModifiedBy).IsRequired(false);
            mb.Entity<Equipment>().Property(c => c.ModifiedDate).IsRequired(false);
            mb.Entity<Equipment>().Property(c => c.CreatedBy).HasDefaultValueSql("NEWID()").IsRequired();
            mb.Entity<Equipment>().Property(c => c.CreatedDate).HasDefaultValueSql("GETUTCDATE()").IsRequired();
            mb.Entity<User>().Property(c => c.ModifiedBy).IsRequired(false);
            mb.Entity<User>().Property(c => c.ModifiedDate).IsRequired(false);
            mb.Entity<User>().Property(c => c.CreatedBy).HasDefaultValueSql("NEWID()").IsRequired();
            mb.Entity<User>().Property(c => c.CreatedDate).HasDefaultValueSql("GETUTCDATE()").IsRequired();
        }

        private void FillWithData()
        {
            var mic1 = new Equipment() { Name = "Microphone 1" };
            var mic2 = new Equipment() { Name = "Microphone 2" };
            var mic3 = new Equipment() { Name = "Microphone 3" };
            var spk1 = new Equipment() { Name = "Speakers 1" };
            var spk2 = new Equipment() { Name = "Speakers 2" };
            var prj1 = new Equipment() { Name = "Projector 1" };
            Equipment.AddRange(mic1, mic2, mic3, spk1, spk2, prj1);

            var david = new User() { FullName = "David Deutsch", Degree = "Clare College, Cambridge (BA) Wolfson College, Oxford(PhD)", Work = "Quantum computation", Position = "Senior worker", ProfessionalBiography = "David Elieser Deutsch FRS[6] (/dɔɪtʃ/ DOYTCH; born 18 May 1953)[1] is a British physicist at the University of Oxford. He is a Visiting Professor in the Department of Atomic and Laser Physics at the Centre for Quantum Computation (CQC) in the Clarendon Laboratory of the University of Oxford. He pioneered the field of quantum computation by formulating a description for a quantum Turing machine, as well as specifying an algorithm designed to run on a quantum computer.[7] He has also proposed the use of entangled states and Bell's theorem for quantum key distribution[7] and is a proponent of the many-worlds interpretation of quantum mechanics." };
            var alan = new User() { FullName = "Alan Guth", Degree = "Bachelor and master", Work = "Theoretical physicist and cosmologist", Position = "Worker", ProfessionalBiography = "Alan Harvey Guth (/ɡuːθ/; born February 27, 1947) is an American theoretical physicist and cosmologist. Guth has researched elementary particle theory (and how particle theory is applicable to the early universe). He is Victor Weisskopf Professor of Physics at the Massachusetts Institute of Technology. Along with Alexei Starobinsky and Andrei Linde, he won the 2014 Kavli Prize for pioneering the theory of cosmic inflation.[1]" };
            var ashoke = new User() { FullName = "Ashoke Sen", Degree = "Presidency College, Kolkata (BSc) IIT Kanpur(MSc) Stony Brook University(PhD)", Work = "Theoretical physicist", Position = "Worker", ProfessionalBiography = "Ashoke Sen FRS (/əˈʃoʊk sɛn/; born 1956) is an Indian theoretical physicist and distinguished professor at the Harish-Chandra Research Institute, Allahabad.[1] He is also an honorary fellow in National Institute of Science Education and Research (NISER), Bhubaneswar, India[2] and also a Morningstar Visiting professor at MIT and a distinguished professor at the Korea Institute for Advanced Study. His main area of work is string theory. He was among the first recipients of the Fundamental Physics Prize for opening the path to the realisation that all string theories are different limits of the same underlying theory.[3]" };
            var steven = new User() { FullName = "Steven Weinberg", Degree = "Cornell University (A.B., 1954) Princeton University(Ph.D., 1957)", Work = "Theoretical physicist", Position = "Worker", ProfessionalBiography = "Ashoke Sen FRS (/əˈʃoʊk sɛn/; born 1956) is an Indian theoretical physicist and distinguished professor at the Harish-Chandra Research Institute, Allahabad.[1] He is also an honorary fellow in National Institute of Science Education and Research (NISER), Bhubaneswar, India[2] and also a Morningstar Visiting professor at MIT and a distinguished professor at the Korea Institute for Advanced Study. His main area of work is string theory. He was among the first recipients of the Fundamental Physics Prize for opening the path to the realisation that all string theories are different limits of the same underlying theory.[3]" };
            var alain = new User() { FullName = "Alain Aspect", Degree = "École Normale Supérieure de Cachan", Work = "Quantum entanglement", Position = "Professor", ProfessionalBiography = "Ashoke Sen FRS (/əˈʃoʊk sɛn/; born 1956) is an Indian theoretical physicist and distinguished professor at the Harish-Chandra Research Institute, Allahabad.[1] He is also an honorary fellow in National Institute of Science Education and Research (NISER), Bhubaneswar, India[2] and also a Morningstar Visiting professor at MIT and a distinguished professor at the Korea Institute for Advanced Study. His main area of work is string theory. He was among the first recipients of the Fundamental Physics Prize for opening the path to the realisation that all string theories are different limits of the same underlying theory.[3]" };
            var tim = new User() { FullName = "Tim Berners-Lee", Degree = "The Queen's College, Oxford (BA)", Work = "Computer scientist", Position = "Worker", ProfessionalBiography = "Sir Timothy John Berners-Lee OM KBE FRS FREng FRSA DFBCS (born 8 June 1955),[1] also known as TimBL, is an English computer scientist best known as the inventor of the World Wide Web. He is a Professorial Fellow of Computer Science at the University of Oxford[2] and a professor at the Massachusetts Institute of Technology (MIT).[3][4] Berners-Lee proposed an information management system on 12 March 1989,[5][6] then implemented the first successful communication between a Hypertext Transfer Protocol (HTTP) client and server via the Internet in mid-November.[7][8][9][10][11]" };
            var jon = new User() { FullName = "Jon Skeet", Degree = "University of Cambridge", Work = "Software engineer", Position = "Worker", ProfessionalBiography = "Jon Skeet is a software engineer known for having the highest reputation on question-and-answer website Stack Overflow." };
            var donald = new User() { FullName = "Donald Knuth", Degree = "Case Institute of Technology (B.S., M.S.) California Institute of Technology(Ph.D.)", Work = "Computer scientist, mathematician", Position = "Professor emeritus", ProfessionalBiography = "Donald Ervin Knuth (/kəˈnuːθ/[3] kə-NOOTH; born January 10, 1938) is an American computer scientist, mathematician, and professor emeritus at Stanford University. He is the 1974 recipient of the ACM Turing Award, informally considered the Nobel Prize of computer science.[4] Knuth has been called the 'father of the analysis of algorithms'.[5]" };
            var ken = new User() { FullName = "Ken Thompson", Degree = "University of California, Berkeley (B.S., M.S.)", Work = "Computer scientist", Position = "Worker", ProfessionalBiography = "Kenneth Lane Thompson (born February 4, 1943) is an American pioneer of computer science. Thompson worked at Bell Labs for most of his career where he designed and implemented the original Unix operating system. He also invented the B programming language, the direct predecessor to the C programming language, and was one of the creators and early developers of the Plan 9 operating system. Since 2006, Thompson has worked at Google, where he co-developed the Go programming language." };
            var john = new User() { FullName = "John Carmack", Degree = "University of Missouri, Kansas City (no degree)", Work = "Computer programmer and video game developer", Position = "Сo-founded", ProfessionalBiography = "John D. Carmack II[1] (born August 20, 1970)[1] is an American computer programmer and video game developer. He co-founded the video game company id Software and was the lead programmer of its 1990s games Commander Keen, Wolfenstein 3D, Doom, Quake, and their sequels. Carmack made innovations in 3D computer graphics, such as his Carmack's Reverse algorithm for shadow volumes. In 2013, he resigned from id to work full-time at Oculus VR, where he served as CTO and later Consulting CTO in 2019.[3]" };
            var niklaus = new User() { FullName = "Niklaus Wirth", Degree = "BS, ETH Zurich(1959) MSc, Université Laval(1960) PhD, University of California, Berkeley(1963)", Work = "Computer scientist", Position = "Worker", ProfessionalBiography = "Niklaus Emil Wirth (born 15 February 1934) is a Swiss computer scientist. He has designed several programming languages, including Pascal, and pioneered several classic topics in software engineering. In 1984, he won the Turing Award, generally recognized as the highest distinction in computer science,[3][4] for developing a sequence of innovative computer languages.[5]" };
            Users.AddRange(david, alan, ashoke, steven, alain, tim, jon, donald, ken, john, niklaus);

            var conference1 = new Conference() { Name = "Conference 1. Physics-based modeling and data representation", Address = "7922 Shadow Brook Circle Tacoma, WA 98444", DateStart = new DateTime(2022, 7, 25, 9, 0, 0), DateEnd = new DateTime(2022, 7, 25, 14, 45, 0) };
            var conference2 = new Conference() { Name = "Conference 2. A New Method of Cockroach Control on Submarines", Address = "7542 Lake View Ave. Willingboro, NJ 08046", DateStart = new DateTime(2022, 7, 25, 13, 0, 0), DateEnd = new DateTime(2022, 7, 25, 19, 50, 0) };
            Conferences.AddRange(conference1, conference2);

            var section1 = new Section() { Name = "A Comparative Acoustic Analysis of Purring in Four Cats", Chairperson = david, Room = "11b", Conference = conference1 };
            var section2 = new Section() { Name = "The Wasted Chewing Gum Bacteriome", Chairperson = tim, Room = "11b", Conference = conference1 };
            var section3 = new Section() { Name = "Cinema Data Mining: The Smell of Fear", Chairperson = ken, Room = "138", Conference = conference2 };
            var section4 = new Section() { Name = "Obesity of Politicians and Corruption in Post‐Soviet Countries", Chairperson = steven, Room = "138", Conference = conference2 };
            var section5 = new Section() { Name = "Impact Protection Potential of Mammalian Hair", Chairperson = niklaus, Room = "138", Conference = conference2 };
            Sections.AddRange(section1, section2, section3, section4, section5);

            var talk1 = new Talk()
            {
                Theme = "The Pulmonary and Metabolic Effects of Suspension",
                Speaker = david,
                Section = section1,
                Equipment = new List<Equipment>() { mic1, spk1 },
                DateStart = new DateTime(2022, 7, 25, 9, 0, 0),
                DateEnd = new DateTime(2022, 7, 25, 9, 45, 0),
            };
            var talk2 = new Talk()
            {
                Theme = "A Chinese Alligator in Heliox",
                Speaker = alan,
                Section = section1,
                Equipment = new List<Equipment>() { mic1, mic2, spk1 },
                DateStart = new DateTime(2022, 7, 25, 10, 0, 0),
                DateEnd = new DateTime(2022, 7, 25, 10, 45, 0),
            };
            var talk3 = new Talk()
            {
                Theme = "Eyebrows Cue Grandiose Narcissism",
                Speaker = ashoke,
                Section = section2,
                Equipment = new List<Equipment>() { mic1, prj1, spk1 },
                DateStart = new DateTime(2022, 7, 25, 11, 0, 0),
                DateEnd = new DateTime(2022, 7, 25, 11, 45, 0),
            };
            var talk4 = new Talk()
            {
                Theme = "Pizza and Risk of Acute Myocardial Infarction",
                Speaker = steven,
                Section = section2,
                Equipment = new List<Equipment>() { mic1, mic2, spk1 },
                DateStart = new DateTime(2022, 7, 25, 12, 30, 0),
                DateEnd = new DateTime(2022, 7, 25, 13, 15, 0),
            };
            var talk5 = new Talk()
            {
                Theme = "Money and Transmission of Bacteria",
                Speaker = alain,
                Section = section2,
                Equipment = new List<Equipment>() { mic1, mic2, spk1 },
                DateStart = new DateTime(2022, 7, 25, 13, 30, 0),
                DateEnd = new DateTime(2022, 7, 25, 14, 45, 0),
            };
            var talk6 = new Talk()
            {
                Theme = "The Pleasurability of Scratching an Itch",
                Speaker = tim,
                Section = section3,
                Equipment = new List<Equipment>() { mic3, spk2 },
                DateStart = new DateTime(2022, 7, 25, 13, 00, 0),
                DateEnd = new DateTime(2022, 7, 25, 13, 50, 0),
            };
            var talk7 = new Talk()
            {
                Theme = "From Data to Truth in Psychological Science",
                Speaker = jon,
                Section = section4,
                Equipment = new List<Equipment>() { mic3, prj1, spk2 },
                DateStart = new DateTime(2022, 7, 25, 14, 00, 0),
                DateEnd = new DateTime(2022, 7, 25, 14, 50, 0),
            };
            var talk8 = new Talk()
            {
                Theme = "The Scent of the Fly",
                Speaker = donald,
                Section = section4,
                Equipment = new List<Equipment>() { mic3, mic2, spk2 },
                DateStart = new DateTime(2022, 7, 25, 15, 00, 0),
                DateEnd = new DateTime(2022, 7, 25, 15, 50, 0),
            };
            var talk9 = new Talk()
            {
                Theme = "Life Is Too Short to RTFM",
                Speaker = donald,
                Section = section4,
                Equipment = new List<Equipment>() { mic3, mic2, spk2 },
                DateStart = new DateTime(2022, 7, 25, 17, 00, 0),
                DateEnd = new DateTime(2022, 7, 25, 17, 50, 0),
            };
            var talk10 = new Talk()
            {
                Theme = "Shouting and Cursing While Driving",
                Speaker = ken,
                Section = section5,
                Equipment = new List<Equipment>() { mic3, prj1, spk2 },
                DateStart = new DateTime(2022, 7, 25, 18, 00, 0),
                DateEnd = new DateTime(2022, 7, 25, 18, 50, 0),
            };
            var talk11 = new Talk()
            {
                Theme = "On the Rheology of Cats",
                Speaker = john,
                Section = section5,
                Equipment = new List<Equipment>() { mic3, mic2, prj1, spk2 },
                DateStart = new DateTime(2022, 7, 25, 19, 00, 0),
                DateEnd = new DateTime(2022, 7, 25, 19, 50, 0),
            };
            Talks.AddRange(talk1, talk2, talk3, talk4, talk5, talk6, talk7, talk8, talk9, talk10, talk11);

            SaveChanges();
        }
    }
}
