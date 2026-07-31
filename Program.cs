namespace ConsoleApp8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using System;

public abstract class Ticket : IPrintable, IBookable, ICloneable
        {
            private static int ticketCounter = 0;
            private int ticketId;
            private string movieName;
            private decimal price;
            private bool booked;

            public int TicketId
            {
                get { return ticketId; }
                private set { ticketId = value; }
            }

            public string MovieName
            {
                get { return movieName; }
                set
                {
                    if (!string.IsNullOrEmpty(value))
                        movieName = value;
                }
            }

            public decimal Price
            {
                get { return price; }
                set
                {
                    if (value > 0)
                        price = value;
                }
            }

            public decimal PriceAfterTax
            {
                get { return Price * 1.14m; }
            }

            public bool IsBooked
            {
                get { return booked; }
                private set { booked = value; }
            }

            public Ticket(string movieName, decimal price)
            {
                ticketCounter++;
                TicketId = ticketCounter;
                MovieName = movieName;
                Price = price;
                IsBooked = false;
            }

            public static int GetTotalTickets()
            {
                return ticketCounter;
            }

            public abstract decimal CalculateFinalPrice();

            public virtual void Print()
            {
                Console.Write($"[Ticket #{TicketId}] {MovieName} | ");
                Console.Write($"Price: {Price:C2} | Final: {CalculateFinalPrice():C2} | Booked: {(IsBooked ? "Yes" : "No")}");
            }

            public bool Book()
            {
                if (!IsBooked)
                {
                    IsBooked = true;
                    return true;
                }
                return false;
            }

            public bool Cancel()
            {
                if (IsBooked)
                {
                    IsBooked = false;
                    return true;
                }
                return false;
            }

            public abstract object Clone();
        }

        public class StandardTicket : Ticket
        {
            private string seatNumber;

            public string SeatNumber
            {
                get { return seatNumber; }
                set { seatNumber = value; }
            }

            public StandardTicket(string movieName, decimal price, string seatNumber) : base(movieName, price)
            {
                SeatNumber = seatNumber;
            }

            public override decimal CalculateFinalPrice()
            {
                return PriceAfterTax;
            }

            public override void Print()
            {
                base.Print();
                Console.Write($" | Seat: {SeatNumber} | Type: Standard");
            }

            public override object Clone()
            {
                StandardTicket clone = new StandardTicket(this.MovieName, this.Price, this.SeatNumber);
                return clone;
            }
        }

        public class VIPTicket : Ticket
        {
            private bool loungeAccess;
            private decimal serviceFee;

            public bool LoungeAccess
            {
                get { return loungeAccess; }
                set { loungeAccess = value; }
            }

            public decimal ServiceFee
            {
                get { return serviceFee; }
                set { serviceFee = value; }
            }

            public VIPTicket(string movieName, decimal price, bool loungeAccess) : base(movieName, price)
            {
                LoungeAccess = loungeAccess;
                ServiceFee = 50;
            }

            public override decimal CalculateFinalPrice()
            {
                return PriceAfterTax + ServiceFee;
            }

            public override void Print()
            {
                base.Print();
                Console.Write($" | Lounge: {(LoungeAccess ? "Yes" : "No")} | Fee: {ServiceFee:C2} | Type: VIP");
            }

            public override object Clone()
            {
                VIPTicket clone = new VIPTicket(this.MovieName, this.Price, this.LoungeAccess);
                return clone;
            }
        }

        public class IMAXTicket : Ticket
        {
            private bool is3D;

            public bool Is3D
            {
                get { return is3D; }
                set { is3D = value; }
            }

            public IMAXTicket(string movieName, decimal price, bool is3D) : base(movieName, price)
            {
                Is3D = is3D;
                if (is3D)
                    Price += 30;
            }

            public override decimal CalculateFinalPrice()
            {
                return PriceAfterTax;
            }

            public override void Print()
            {
                base.Print();
                Console.Write($" | 3D: {(Is3D ? "Yes" : "No")} | Type: IMAX");
            }

            public override object Clone()
            {
                IMAXTicket clone = new IMAXTicket(this.MovieName, this.Price, this.Is3D);
                return clone;
            }
        }

        public interface IPrintable
        {
            void Print();
        }

        public interface IBookable
        {
            bool Book();
            bool Cancel();
            bool IsBooked { get; }
        }

        public class Projector
        {
            private bool isOn;

            public Projector()
            {
                isOn = false;
            }

            public void TurnOn()
            {
                isOn = true;
                Console.WriteLine("Projector ON");
            }

            public void TurnOff()
            {
                isOn = false;
                Console.WriteLine("Projector OFF");
            }

            public bool IsOn()
            {
                return isOn;
            }
        }

        public partial class Cinema
        {
            private string cinemaName;
            private Projector projector;
            private Ticket[] tickets;
            private int ticketCount;

            public string CinemaName
            {
                get { return cinemaName; }
                set
                {
                    if (!string.IsNullOrEmpty(value))
                        cinemaName = value;
                }
            }

            public Cinema(string cinemaName)
            {
                CinemaName = cinemaName;
                projector = new Projector();
                tickets = new Ticket[20];
                ticketCount = 0;
            }

            public bool AddTicket(Ticket t)
            {
                if (ticketCount < tickets.Length)
                {
                    tickets[ticketCount] = t;
                    ticketCount++;
                    return true;
                }
                return false;
            }

            public void OpenCinema()
            {
                Console.WriteLine($"=== {CinemaName} Opened ===");
                projector.TurnOn();
                Console.WriteLine();
            }

            public void CloseCinema()
            {
                Console.WriteLine($"=== {CinemaName} Closed ===");
                projector.TurnOff();
                Console.WriteLine();
            }
        }

        public partial class Cinema
        {
            public void PrintAllTickets()
            {
                Console.WriteLine("--- All Tickets (from Cinema.Reporting) ---");
                for (int i = 0; i < ticketCount; i++)
                {
                    if (tickets[i] != null)
                    {
                        tickets[i].Print();
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
            }

            public void PrintPolymorphism()
            {
                Console.WriteLine("--- Polymorphism: Final Price per Ticket ---");
                for (int i = 0; i < ticketCount; i++)
                {
                    if (tickets[i] != null)
                    {
                        string typeName = tickets[i].GetType().Name;
                        Console.WriteLine($"{typeName} => Final Price: {tickets[i].CalculateFinalPrice():C2}");
                    }
                }
                Console.WriteLine();
            }
        }

        public static class TicketExtensions
        {
            public static string GenerateReceipt(this Ticket ticket)
            {
                string receipt = "========== RECEIPT ==========\n";
                receipt += $"  Movie    : {ticket.MovieName}\n";
                receipt += $"  Type     : {ticket.GetType().Name}\n";
                receipt += $"  Price    : {ticket.Price:C2}\n";
                receipt += $"  Final    : {ticket.CalculateFinalPrice():C2}\n";
                receipt += $"  Status   : {(ticket.IsBooked ? "Booked" : "Not Booked")}\n";
                receipt += "=============================";
                return receipt;
            }

            public static decimal CalculateTotalRevenue(this Ticket[] tickets)
            {
                decimal total = 0;
                foreach (Ticket t in tickets)
                {
                    if (t != null && t.IsBooked)
                        total += t.CalculateFinalPrice();
                }
                return total;
            }
        }

        class Program
        {
            static void Main()
            {
                Cinema cinema = new Cinema("Grand Cinema");
                cinema.OpenCinema();

                StandardTicket standard = new StandardTicket("Inception", 80, "A5");
                VIPTicket vip = new VIPTicket("Avengers", 200, true);
                IMAXTicket imax = new IMAXTicket("Dune", 100, true);

                standard.Book();
                vip.Book();
                imax.Book();

                cinema.AddTicket(standard);
                cinema.AddTicket(vip);
                cinema.AddTicket(imax);

                cinema.PrintAllTickets();

                cinema.PrintPolymorphism();

                Console.WriteLine("--- Extension Method: Receipt ---");
                Console.WriteLine(vip.GenerateReceipt());
                Console.WriteLine();

                Ticket[] allTickets = new Ticket[] { standard, vip, imax };
                Console.WriteLine("--- Extension Method: Total Revenue ---");
                Console.WriteLine($"Total Revenue: {allTickets.CalculateTotalRevenue():C2}");
                Console.WriteLine();

                cinema.CloseCinema();
            }
        }
    }
    }
}
