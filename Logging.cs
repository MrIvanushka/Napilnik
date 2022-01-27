using System;
using System.Collections.Generic;
using System.IO;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            Pathfinder fileLogWriter = new Pathfinder(new FileLogWriter());
            Pathfinder consoleLogWriter = new Pathfinder(new ConsoleLogWriter());
            Pathfinder securedFileLogWriter = new Pathfinder(new SecureLogWriter(new FileLogWriter()));
            Pathfinder securedConsoleLogWriter = new Pathfinder(new SecureLogWriter(new ConsoleLogWriter()));
            Pathfinder customLogWriter = new Pathfinder(new ConsoleLogWriter(), new SecureLogWriter(new FileLogWriter()));
        }
    }
    
    interface ILogWriter
    {
        void Find(string message);
    }

    class ConsoleLogWriter : ILogWriter
    {
        public virtual void Find(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWriter : ILogWriter
    {
        public virtual void Find(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class Pathfinder : ILogWriter
    {
        private IEnumerable<ILogWriter> _loggers;

        public Pathfinder(params ILogWriter[] loggers)
        {
            if (loggers == null)
                throw new ArgumentNullException();

            _loggers = loggers;
        }

        public virtual void Find(string message)
        {
            foreach(var logger in _loggers)
                logger.Find(message);
        }
    }
    
    class SecureLogWriter : Pathfinder
    {
        public SecureLogWriter(params ILogWriter[] loggers) : base(loggers)
        {  }

        public override void Find(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                base.Find(message);
            }
        }
    }
}
