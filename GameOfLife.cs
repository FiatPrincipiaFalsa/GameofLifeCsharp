using System;
public class program
{
	public static void Main()
	{
		Console.Clear();
		Console.WriteLine("Hello, Welcome to Jonnys Game of Life.");
		Console.WriteLine("Natural amount of sleep inbetween generations(ms):");
		int downtime = Convert.ToInt32(Console.ReadLine());
		Console.WriteLine("Natural amount of generations to be simulated:");
		int generationlim = Convert.ToInt32(Console.ReadLine());
		Console.WriteLine("Natural size of square grid for simulation:");
		int Length = Convert.ToInt32(Console.ReadLine());
		int Area = Length * Length;
		Console.Clear();

		//Set up initial state
		string[,] state = new string[Length, Length];
		int a = 0;
		while(a < Area)
		{
			state[a / Length, a % Length] = "-";
			a++;
		}

		//input initial state
		int b = 0;
		while(b < Area)
		{
			state[b / Length, b % Length] = "x";
			int c = 0;
			while(c < Area)
			{	
				Console.Write(state[c / Length, c % Length]);
				if((c + 1) % Length == 0)Console.Write("\n");
				c++;
			}
			string input = "";
			while(input != "0" && input != "1")
			{
				Console.WriteLine("\nPlease enter state 1/(0)");
				input = Console.ReadLine();
				if(input == "")
				{
					input = "0";
				}
			}
			state[b / Length, b % Length] = input;
			Console.Clear();
			b++;
		}
		
		//simulation of the automita
		int generation = 0;
		while(generation < generationlim + 1)
		{
			string[,] nextstate = new string[Length, Length];
			int d = 0;
			while(d < Area)
			{
				int pop = 0;
				int e = 0;
				while(e < 9)
				{
					if(((d / Length) - 1) + (e / 3) < 0 || ((d / Length) - 1) + (e / 3) > (Length - 1) || ((d % Length) - 1) + (e % 3) < 0 || ((d % Length) - 1) + (e % 3) > (Length - 1))
					{
					}
					else
					{
						if(state[((d / Length) - 1) + (e / 3), ((d % Length) - 1) + (e % 3)] == "1" && e != 4 )
						{
							pop++;
						}
					}
					e++;
				}
				if(state[d / Length, d % Length] == "1" && ( pop < 2 || pop > 3))
				{
					nextstate[d / Length, d % Length] = "0";
				}
				if(state[d / Length, d % Length] == "1" && ( pop == 2 || pop == 3))
				{
					nextstate[d / Length , d % Length] = "1";
				}
				if(state[d / Length, d % Length] == "0" && pop == 3)
				{
					nextstate[d / Length, d % Length] = "1";
				}
				if (state[d / Length, d % Length] == "0" && pop != 3)
				{
					nextstate[d / Length, d % Length] = "0";
				}
				d++;

			}
			Console.Clear();
			int f = 0;
			while(f < Area)
			{
				Console.Write(nextstate[f / Length, f % Length]);
				if((f + 1) % Length == 0)Console.Write("\n");
				f++;
			}
			Console.Write(Convert.ToString(generation) + "/" + Convert.ToString(generationlim));
			System.Threading.Thread.Sleep(downtime);
			Console.Write("\n\n\n");
			generation++;
			state = nextstate;
		}
	}
}
