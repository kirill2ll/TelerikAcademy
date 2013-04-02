using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyEcosystem
{
public enum AnimalState
    {
        Sleeping,
        Awake
    }

	public struct Point
    {
        public readonly int X;

        public readonly int Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point(string xString, string yString)
        {
            this.X = int.Parse(xString);
            this.Y = int.Parse(yString);
        }

        public override int GetHashCode()
        {
            return this.X * 7 + this.Y;
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return String.Format("({0},{1})", this.X, this.Y);
        }

        public static Point Parse(string pointString)
        {
            string coordinatesPairString = pointString.Substring(1, pointString.Length - 2);
            string[] coordinates = coordinatesPairString.Split(',');
            return new Point(coordinates[0], coordinates[1]);
        }

        public static int GetManhattanDistance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
	
	public abstract class Animal : Organism
    {
        protected int sleepRemaining;

        public AnimalState State { get; protected set; }

        public string Name { get; private set; }

        protected Animal(string name, Point location, int size)
            : base(location, size)
        {
            this.Name = name;
            this.sleepRemaining = 0;
        }

        public virtual int GetMeatFromKillQuantity()
        {
            this.IsAlive = false;
            return this.Size;
        }

        public void GoTo(Point destination)
        {
            this.Location = destination;
            if (this.State == AnimalState.Sleeping)
            {
                this.Awake();
            }
        }

        public void Sleep(int time)
        {
            this.sleepRemaining = time;
            this.State = AnimalState.Sleeping;
        }

        protected void Awake()
        {
            this.sleepRemaining = 0;
            this.State = AnimalState.Awake;
        }

        public override void Update(int timeElapsed)
        {
            if (this.sleepRemaining == 0)
            {
                this.Awake();
            }
        }

        public override string ToString()
        {
            return base.ToString() + " " + this.Name;
        }
    }
	
	public interface ICarnivore
    {
        int TryEatAnimal(Animal animal);
    }
	
	public interface IHerbivore
    {
        int EatPlant(Plant plant);
    }
	
	public interface IOrganism
    {
        bool IsAlive { get; }

        Point Location { get; }

        int Size { get; }

        void Update(int timeElapsed);
    }
	
	public abstract class Organism : IOrganism
    {
        public bool IsAlive {get; protected set;}

        public Point Location { get; protected set; }

        public int Size { get; protected set; }

        public Organism(Point location, int size)
        {
            this.Location = location;
            this.Size = size;
            this.IsAlive = true;
        }

        public virtual void Update(int time)
        {
        }

        public virtual void RespondTo(IOrganism organism)
        {
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
	
	public abstract class Plant : Organism
    {
        protected Plant(Point location, int size)
            : base(location, size)
        {
        }

        public int GetEatenQuantity(int biteSize)
        {
            if (biteSize > this.Size)
            {
                this.IsAlive = false;
                this.Size = 0;
                return this.Size;
            }
            else
            {
                this.Size -= biteSize;
                return biteSize;
            }
        }

        public override string ToString()
        {
            return base.ToString() + " " + this.Location;
        } 
    }
	
	public class Tree : Plant
    {
        public Tree(Point location)
            : base(location, 15)
        {
        }
    }
	
	public class Bush : Plant
    {
        public Bush(Point location)
            : base(location, 4)
        {
        }
    }
	
	public class Grass : Plant
    {
        public Grass(Point location)
            : base(location, 2)
        {
        }

        public override void Update(int time)
        {
            if (this.IsAlive) this.Size += time;
        }
    }
	
	public class Boar : Animal, IHerbivore, ICarnivore
    {
        private int biteSize;

        public Boar(string name, Point location)
            : base(name, location, 4)
        {
            biteSize = 2;
        }

        public int TryEatAnimal(Animal animal)
        {
            if (animal != null)
            {
                if (animal.Size <= this.Size)
                {
                    this.Size++;
                    return animal.GetMeatFromKillQuantity();
                }
            }

            return 0;
        }

        public int EatPlant(Plant p)
        {
            if (p != null)
            {
                this.Size++;
                return p.GetEatenQuantity(this.biteSize);
            }

            return 0;
        }
    }
	
	public class Lion : Animal, ICarnivore
    {
        public Lion(string name, Point location)
            : base(name, location, 6)
        {
        }

        public int TryEatAnimal(Animal animal)
        {
            if (animal != null)
            {
                if (animal.Size <= this.Size * 2)
                {
                    this.Size++;
                    return animal.GetMeatFromKillQuantity();
                }
            }

            return 0;
        }
    }
	
	public class Wolf : Animal, ICarnivore
    {
        public Wolf(string name, Point location)
            : base(name, location, 4)
        {
        }

        public int TryEatAnimal(Animal animal)
        {
            if (animal != null)
            {
                if (animal.Size <= this.Size || animal.State == AnimalState.Sleeping)
                {
                    return animal.GetMeatFromKillQuantity();
                }
            }

            return 0;
        }
    }
	
	public class Deer : Animal, IHerbivore
    {
        private int biteSize;

        public Deer(string name, Point location)
            : base(name, location, 3)
        {
            this.biteSize = 1;
        }

        public override void Update(int timeElapsed)
        {
            if (this.State == AnimalState.Sleeping)
            {
                if (timeElapsed >= sleepRemaining)
                {
                    this.Awake();
                }
                else
                {
                    this.sleepRemaining -= timeElapsed;
                }
            }

            base.Update(timeElapsed);
        }

        public int EatPlant(Plant p)
        {
            if (p != null)
            {
                return p.GetEatenQuantity(this.biteSize);
            }

            return 0;
        }
    }
	
	public class Zombie : Animal
    {
        private static int ZombieMeatQuantity = 10;

        public Zombie(string name, Point location)
            : base(name, location, 0)
        {
        }

        public override int GetMeatFromKillQuantity()
        {
            return Zombie.ZombieMeatQuantity;
        }
    }
	
	public class Engine
    {
        protected static readonly char[] separators = new char[] { ' ' };

        protected List<Organism> allOrganisms;
        protected List<Plant> plants;
        protected List<Animal> animals;

        public Engine()
        {
            this.allOrganisms = new List<Organism>();
            this.plants = new List<Plant>();
            this.animals = new List<Animal>();
        }

        public void AddOrganism(Organism organism)
        {
            this.allOrganisms.Add(organism);

            var organismAsAnimal = organism as Animal;
            var organismAsPlant = organism as Plant;

            if (organismAsAnimal != null)
            {
                this.animals.Add(organismAsAnimal);
            }

            if (organismAsPlant != null)
            {
                this.plants.Add(organismAsPlant);
            }
        }

        public void ExecuteCommand(string command)
        {
            string[] commandWords = command.Split(Engine.separators, StringSplitOptions.RemoveEmptyEntries);

            if (commandWords[0] == "birth")
            {
                this.ExecuteBirthCommand(commandWords);
            }
            else
            {
                this.ExecuteAnimalCommand(commandWords);
            }

            this.RemoveAndReportDead();
        }

        protected virtual void ExecuteBirthCommand(string[] commandWords)
        {
            switch (commandWords[1])
            {
                case "deer":
                    {
                        string name = commandWords[2];
                        Point position = Point.Parse(commandWords[3]);
                        this.AddOrganism(new Deer(name, position));
                        break;
                    }
                case "tree":
                    {
                        Point position = Point.Parse(commandWords[2]);
                        this.AddOrganism(new Tree(position));
                        break;
                    }
                case "bush":
                    {
                        Point position = Point.Parse(commandWords[2]);
                        this.AddOrganism(new Bush(position));
                        break;
                    }
            }
        }

        protected virtual void ExecuteAnimalCommand(string[] commandWords)
        {
            switch (commandWords[0])
            {
                case "go":
                    {
                        string name = commandWords[1];
                        Point destination = Point.Parse(commandWords[2]);
                        destination = HandleGo(name, destination);
                        break;
                    }
                case "sleep":
                    {
                        string name = commandWords[1];
                        int sleepTime = int.Parse(commandWords[2]);
                        HandleSleep(name, sleepTime);
                        break;
                    }
            }
        }

        private Point HandleGo(string name, Point destination)
        {
            Animal current = GetAnimalByName(name);

            if (current != null)
            {
                int travelTime = Point.GetManhattanDistance(current.Location, destination);
                this.UpdateAll(travelTime);
                current.GoTo(destination);

                HandleEncounters(current);
            }
            return destination;
        }

        private void HandleEncounters(Animal current)
        {
            List<Organism> allEncountered = new List<Organism>();
            foreach (var organism in this.allOrganisms)
            {
                if (organism.Location == current.Location && !(organism == current))
                {
                    allEncountered.Add(organism);
                }
            }

            var currentAsHerbivore = current as IHerbivore;
            if (currentAsHerbivore != null)
            {
                foreach (var encountered in allEncountered)
                {
                    int eatenQuantity = currentAsHerbivore.EatPlant(encountered as Plant);
                    if (eatenQuantity != 0)
                    {
                        Console.WriteLine("{0} ate {1} from {2}", current, eatenQuantity, encountered);
                    }
                }
            }

            allEncountered.RemoveAll(o => !o.IsAlive);

            var currentAsCarnivore = current as ICarnivore;
            if (currentAsCarnivore != null)
            {
                foreach (var encountered in allEncountered)
                {
                    int eatenQuantity = currentAsCarnivore.TryEatAnimal(encountered as Animal);
                    if (eatenQuantity != 0)
                    {
                        Console.WriteLine("{0} ate {1} from {2}", current, eatenQuantity, encountered);
                    }
                }
            }

            this.RemoveAndReportDead();
        }

        private void HandleSleep(string name, int sleepTime)
        {
            Animal current = GetAnimalByName(name);
            if (current != null)
            {
                current.Sleep(sleepTime);
            }
        }

        private Animal GetAnimalByName(string name)
        {
            Animal current = null;
            foreach (var animal in this.animals)
            {
                if (animal.Name == name)
                {
                    current = animal;
                    break;
                }
            }
            return current;
        }

        protected virtual void RemoveAndReportDead()
        {
            foreach (var organism in this.allOrganisms)
            {
                if (!organism.IsAlive)
                {
                    Console.WriteLine("{0} is dead ;(", organism);
                }
            }

            this.allOrganisms.RemoveAll(o => !o.IsAlive);
            this.plants.RemoveAll(o => !o.IsAlive);
            this.animals.RemoveAll(o => !o.IsAlive);
        }

        protected virtual void UpdateAll(int timeElapsed)
        {
            foreach (var organism in this.allOrganisms)
            {
                organism.Update(timeElapsed);
            }
        }
    }
	
	public class AdvancedEngine : Engine
    {
        protected override void ExecuteBirthCommand(string[] commandWords)
        {
            switch (commandWords[1])
            {
                case "wolf":
                    {
                        string name = commandWords[2];
                        Point position = Point.Parse(commandWords[3]);
                        this.AddOrganism(new Wolf(name, position));
                        break;
                    }
                case "lion":
                    {
                        string name = commandWords[2];
                        Point position = Point.Parse(commandWords[3]);
                        this.AddOrganism(new Lion(name, position));
                        break;
                    }
                case "grass":
                    {
                        Point position = Point.Parse(commandWords[2]);
                        this.AddOrganism(new Grass(position));
                        break;
                    }

                case "boar":
                    {
                        string name = commandWords[2];
                        Point position = Point.Parse(commandWords[3]);
                        this.AddOrganism(new Boar(name, position));
                        break;
                    }

                case "zombie":
                    {
                        string name = commandWords[2];
                        Point position = Point.Parse(commandWords[3]);
                        this.AddOrganism(new Zombie(name, position));
                        break;
                    }
            }

            base.ExecuteBirthCommand(commandWords);
        }
    }

    class Program
    {
        static Engine GetEngineInstance()
        {
            return new AdvancedEngine();
        }

        static void Main(string[] args)
        {
            Engine engine = GetEngineInstance();

            string command = Console.ReadLine();
            while (command != "end")
            {
                engine.ExecuteCommand(command);
                command = Console.ReadLine();
            }
        }
    }
}
