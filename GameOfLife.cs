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
		while(b<Area)
		{
			state[b/Length,b%Length] = "x";
			int c = 0;
			while ( c<Area)
			{	
				Console.Write(state[c/Length,c%Length]);
				if ((c+1)%Length == 0)Console.Write("\n");
				c++;
			}
			string input = "";
			while (input != "0" && input != "1")
			{
				Console.WriteLine("\nPlease enter state 1/0");
				input = Console.ReadLine();
			}
			state[b / Length , b % Length] = input;
			Console.Clear();
			b++;
		}









		//print grid    not sure if needed???
		int d = 0;
		while(d<Area)
		{
			Console.Write(state[d/Length,d%Length]);
			if ((d+1)%Length == 0)Console.Write("\n");
			d++;
		}
		
		
		






		int generation = 0;
		while(generation<generationlim+1)
		{
			string[,] nextstate = new string[Length,Length];
			int k = 0;
			while(k<Area)
			{
				int pop = 0;
				int l = 0;
				while(l<9)
				{
					if ( ((k/Length)-1)+(l/3) < 0 || ((k/Length)-1)+(l/3) > (Length - 1) || ((k%Length)-1)+(l%3) < 0 || ((k%Length)-1)+(l%3) >(Length - 1))
					{
					}
					else
					{
						if (state[((k/Length)-1)+(l/3),((k%Length)-1)+(l%3)] == "1" && l!=4 )
						{
							pop++;
						}
					}
					l++;
				}
				if (state[k/Length,k%Length] == "1" &&( pop<2 || pop>3))
				{
					nextstate[k/Length,k%Length] = "0";
				}
				if (state[k/Length,k%Length] == "1" &&( pop==2 || pop==3))
				{
					nextstate[k/Length,k%Length] = "1";
				}
				if (state[k/Length,k%Length] == "0" && pop == 3)
				{
					nextstate[k/Length,k%Length] = "1";
				}
				if (state[k/Length,k%Length] == "0" && pop != 3)
				{
					nextstate[k/Length,k%Length] = "0";
				}
				k++;

			}
			Console.Clear();
			int m = 0;
			while(m<Area)
			{
				Console.Write(nextstate[m/Length,m%Length]);
				if((m+1)%Length == 0)Console.Write("\n");
				m++;
			}
			Console.Write(Convert.ToString(generation)+"/"+Convert.ToString(generationlim));
			System.Threading.Thread.Sleep(downtime);
			Console.Write("\n\n\n");
			generation++;
			state = nextstate;
		}
	}
}
