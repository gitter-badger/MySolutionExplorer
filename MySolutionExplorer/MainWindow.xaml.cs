﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Build.Construction;
using Microsoft.Build;
using Microsoft.Build.Exceptions;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Logging;
using System.Xml;
using System.Xml.Serialization;

namespace MySolutionExplorer
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			var dir = new DirectoryInfo(@"C:\Users\vladislav\OneDrive\Projects\MySolutionExplorer\ExperimentalSolution\");
			//var s = SolutionFile.Parse(@"C:\Users\vladislav\OneDrive\Projects\MySolutionExplorer\ExperimentalSolution\ExperimentalSolution.sln");
			//s.ProjectsInOrder[0].Dependencies.
			//var p2 = new MyProject(dir.FullName + @"acmp 0156. Шахматы - 2 cs\acmp 0156. Шахматы - 2 cs.csproj");
			//var f = dir.GetFiles().
			//var d = dir.GetDirectories();

			//var p1 = new MyProject(dir.FullName + @"acmp 0114. Без двух нулей подряд\acmp 0114. Без двух нулей подряд.vcxproj");
			//var p = new MyProject(dir.FullName + @"acmp 0001. A+B [cpp]\acmp 0001. A+B [cpp].vcxproj");
			//p.VSLastProj.AddItem("txt", dir.FullName + @"acmp 0001. A+B [cpp]\input.txt");


			//var x = XmlReader.Create(dir + "textxml.xml");
			//var x = new XmlDocument();
			//x.Load(dir + "textxml.xml");
			//x.DocumentElement["PropertyGroup"]["RootNamespace"].InnerText = "acmp_0001";
			//x.Save(dir + "textxml.xml");


			var p = new CppProject(dir + @"acmp 0001. A+B [cpp]")
			{
				Name = "A+B",
				Site = "acmp",
				Number = 1,
				Lang = "cpp"
			}; 
			p.FindFiles();
			p.FindProjectFiles();
			
			var p1 = new Project(dir + @"acmp 0156. Шахматы - 2 cs")
			{
				Name = "Шахматы - 2",
				Site = "acmp",
				Number = 156,
				Lang = "cs"
			};


			var s = new Solution(dir.FullName);
			s.Add(p);
			s.Add(p1);

			var serializer = new XmlSerializer(typeof(Solution));

			using (var fs = new StreamWriter(dir + "ExperimentalSolution.mysln"))
			{
				serializer.Serialize(fs, s);
				return;
			}

			//using (FileStream fs = new FileStream("people.xml", FileMode.OpenOrCreate))
			//{
			//	Person[] newpeople = (Person[])formatter.Deserialize(fs);

			//	foreach (Person p in newpeople)
			//	{
			//		Console.WriteLine("Имя: {0} --- Возраст: {1}", p.Name, p.Age);
			//	}
			//}
		}
	}
}
