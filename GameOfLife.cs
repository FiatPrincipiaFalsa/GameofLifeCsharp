using System;
public class program
{
	public static void Main()
	{
		Console.Clear();
		Console.WriteLine("Hello, Welcome to Jonnys Game of Life.\nNatural amount of sleep inbetween generations(ms):");
		int downtime = Convert.ToInt32(Console.ReadLine());
		Console.WriteLine("Natural amount of generations to be simulated:");
		int generationlim = Convert.ToInt32(Console.ReadLine());
		Console.WriteLine("Natural size of square grid for simulation:");
		int Size = Convert.ToInt32(Console.ReadLine());
		Console.Clear();

		string[,] state = {{"-","-","-"},{"-","-","-"},{"-","-","-"}};
		


		int setup = 0;
		while(setup<9)
		{
			state[setup/3,setup%3] = "x";
		/////////////////////////////////////////////print matrix
			int i = 0;
			while ( i<9 )
			{	
				Console.Write(state[i/3,i%3]);
				if ((i+1)%3 == 0)Console.Write("\n");
				i++;
			}
		/////////////////////////////////////////////end print matrix  REMINDERRR, collapse all 3 different print cycle somehow, idk?????
		/////////////////////////////////////////////matrix input
			string input = "";
			while (input != "0" && input != "1")
			{
				Console.WriteLine("\nPlease enter state 1/0");
				input = Console.ReadLine();
			}
			state[setup / 3 , setup % 3] = input;
			Console.Clear();
			setup++;
		}
		/////////////////////////////////////////////end matrix input
		/////////////////////////////////////////////print matrix
		int j = 0;
		while(j<9)
		{
			Console.Write(state[j/3,j%3]);
			if ((j+1)%3 == 0)Console.Write("\n");
			j++;
		}
		//////////////////////////////////////////////end pri
		
		
		int generation = 0;
		while(generation<generationlim+1)
		{
			string[,] nextstate = new string[3,3];
			int k = 0;
			while(k<9)
			{
				int pop = 0;
				int l = 0;
				while(l<9)
				{
					if ( ((k/3)-1)+(l/3) < 0 || ((k/3)-1)+(l/3) > 2 || ((k%3)-1)+(l%3) < 0 || ((k%3)-1)+(l%3) >2)
					{
					}
					else
					{
						if (state[((k/3)-1)+(l/3),((k%3)-1)+(l%3)] == "1" && l!=4 )
						{
							pop++;
						}
					}
					l++;
				}
				if (state[k/3,k%3] == "1" &&( pop<2 || pop>3))
				{
					nextstate[k/3,k%3] = "0";
				}
				if (state[k/3,k%3] == "1" &&( pop==2 || pop==3))
				{
					nextstate[k/3,k%3] = "1";
				}
				if (state[k/3,k%3] == "0" && pop == 3)
				{
					nextstate[k/3,k%3] = "1";
				}
				if (state[k/3,k%3] == "0" && pop != 3)
				{
					nextstate[k/3,k%3] = "0";
				}
				k++;

			}
			Console.Clear();
			int m = 0;
			while(m<9)
			{
				Console.Write(nextstate[m/3,m%3]);
				if((m+1)%3 == 0)Console.Write("\n");
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
