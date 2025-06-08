using System;
public class program
{
	static bool Wrap;
	static bool Settup;
	static int AliveNeighbourCount(int Row, int Column, string[,] State)
	{
		int AliveCount = 0; 
		for (int Neighbour = 0 ; Neighbour < 9 ; Neighbour++ )
		{	
			int NeighbourRow = Row + ((Neighbour / 3) - 1);
			int NeighbourColumn = Column + ((Neighbour % 3) - 1);
			if (Wrap)
			{
				NeighbourRow = (NeighbourRow + State.GetLength(0)) % (State.GetLength(0));
				NeighbourColumn = (NeighbourColumn + State.GetLength(0)) % (State.GetLength(0));
			}

			if (NeighbourRow == Row && NeighbourColumn == Column)
			{
				continue;
			}

			if (IsCellAlive(NeighbourRow, NeighbourColumn, State))
			{
				AliveCount++;
			}
		}
		return AliveCount;

	}

	static bool IsCellAlive(int Row ,int Column, string[,] State)
	{
		if (
			Row < 0
			|| Row > (State.GetLength(0) - 1)
			|| Column < 0
			|| Column > (State.GetLength(0) - 1)
		)
		{
			return false;
		}
		return State[Row, Column] == "\u25a0 " ;
	}

	static int AskSleep()
	{
		Console.WriteLine("Natural amount of sleep inbetween generations (50 ms):");
		string Downtime = Console.ReadLine();
		if( Downtime != "")
		{
			return Convert.ToInt32(Downtime);
		}
		return 50;	
	}

	static int AskGenerations()
	{
		Console.WriteLine("Natural amount of generations to be simulated (200 Gemerations):");
		string GenerationLimit = Console.ReadLine();
		if(GenerationLimit != "")
		{
			return Convert.ToInt32(GenerationLimit);
		}
		return 200;
	}

	static int AskMatrixSize()
	{
		Console.WriteLine("Natural size of square grid for simulation (10):");
		string Length = Console.ReadLine();
		if(Length != "")
		{
			return  Convert.ToInt32(Length);
		}
		return 10;
	}

	static bool AskWrap()
	{
		Console.Write("wrapped border or dead border (W/d):");
		if(Console.ReadLine() == "d")
		{
			return false;
		}
		return true;
	}

	static string[,] InitialiseState(int Length, int Area)
	{
		string[,] State = new string[Length, Length];
		int i = 0;
		while(i < Area)
		{
			State[i / Length, i % Length] = "\u25a1 ";
			i++;
		}
		return State;
	}

	static void DrawState(int Length, int Area, string[,] State)
	{
		int i = 0;
		while(i < Area)
		{	
			Console.Write(State[i / Length, i % Length]);
			if((i + 1) % Length == 0)Console.Write("\n");
			i++;
		}
	}

	static int ReadKeyPressed(int Cursor, int Length, int Area, string Hold, string[,] State)
	{
		var ch = Console.ReadKey(false).Key;
		switch(ch)
		{
			case ConsoleKey.RightArrow:
				State[Cursor / Length, Cursor % Length] = Hold;
				Console.Clear();
				if(Cursor == Area - 1)
					Cursor = 0;
				else
					Cursor = Cursor + 1;
				break;
			case ConsoleKey.LeftArrow:
				State[Cursor / Length, Cursor % Length] = Hold;
				Console.Clear();
				if(Cursor == 0)
					Cursor = Area - 1;
				else
					Cursor = Cursor - 1;
				break;
			case ConsoleKey.UpArrow:
				State[Cursor / Length, Cursor % Length] = Hold;
				Console.Clear();
				if(Cursor / Length == 0)
					Cursor = Cursor + Length * (Length - 1);
				else
					Cursor = Cursor - Length;
				break;
			case ConsoleKey.DownArrow:
				State[Cursor / Length, Cursor % Length] = Hold;
				Console.Clear();
				if(Cursor / Length == Length - 1)
					Cursor = Cursor - Length * (Length - 1);
				else
					Cursor = Cursor + Length;
				break;
			case ConsoleKey.X:
				if (Hold == "\u25a1 ")
					State[Cursor / Length, Cursor % Length] = "\u25a0 ";
				else
					State[Cursor / Length, Cursor % Length] = "\u25a1 ";
				Console.Clear();
				break;
			case ConsoleKey.Enter:
				State[Cursor / Length , Cursor % Length] = Hold;
				Console.Clear();
				Settup = false;
				break;
			default:
				State[Cursor / Length , Cursor % Length] = Hold;
				Console.Clear();
				break;
		}
		return Cursor;
	}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	static string[,] GenerateNextState(int Length, int Area, string[,] State)
	{
		string[,] NextState = new string[Length, Length];
		for(int i = 0; i < Area; i++)
		{
			int Row = i / Length;
			int Column = i % Length;
			int Pop = AliveNeighbourCount(Row, Column, State);

			if(State[Row, Column] == "\u25a0 " && ( Pop < 2 || Pop > 3))
			{
				NextState[Row, Column] = "\u25a1 ";
				Console.SetCursorPosition(2*Column,Row);
				Console.Write("\u25a1 ");
				continue;
			}
			if(State[Row, Column] == "\u25a1 " && Pop == 3)
			{
				NextState[Row, Column] = "\u25a0 ";
				Console.SetCursorPosition(2*Column,Row);
				Console.Write("\u25a0 ");
				continue;
			}
			else 
			{
				NextState[Row, Column] = State[Row, Column];
			}
		}
		return NextState;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////		
	}
	public static void Main()
	{
		Console.Clear();
		int Downtime = AskSleep();
		int Generationlim = AskGenerations();
		int Length = AskMatrixSize();
		int Area = Length * Length;
		Wrap = AskWrap();
		Console.Clear();
		
		string[,] State = InitialiseState(Length, Area);
		
		int Cursor = 0;
		Settup = true;
		while(Settup)
		{
			string Hold = State[Cursor / Length, Cursor % Length];
			State[Cursor / Length, Cursor % Length] = "X ";

			DrawState(Length, Area, State);
			Console.WriteLine("(" + Hold + ")\nToggle (x)\nExecute(Rtn)");

			Cursor = ReadKeyPressed(Cursor, Length, Area, Hold, State);
		}

						
		DrawState(Length, Area, State);
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////		
		for(int Generation = 0; Generation < (Generationlim + 1); Generation++)
		{
			string[,] NextState = GenerateNextState(Length, Area, State);	
			System.Threading.Thread.Sleep(Downtime);
			State = NextState;
		}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		Console.Clear();
	}
}
