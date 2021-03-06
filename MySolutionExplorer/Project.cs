﻿// Copyright (c) 2017 Vladislav Prekel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MySolutionExplorer
{
	/// <summary>
	/// Проект
	/// </summary>
	[Serializable]
	public abstract class Project
	{
		private DirectoryInfo _dir;

		/// <summary>
		/// Директория проекта
		/// </summary>
		[XmlIgnore]
		public DirectoryInfo Dir
		{
			get
			{
				if (_dir != null)
				{
					return _dir;
				}
				return _dir = new DirectoryInfo(ParentSolution.Dir + MyEnum.Slash + Name);
			}
			set { _dir = value; }
		}

		/// <summary>
		/// Файл с кодом
		/// </summary>
		[XmlIgnore]
		public FileInfo CodeFile { get; set; }

		/// <summary>
		/// Файл ввода (input.txt)
		/// </summary>
		[XmlIgnore]
		public FileInfo InputFile { get; set; }

		/// <summary>
		/// Файл вывода (output.txt)
		/// </summary>
		[XmlIgnore]
		public FileInfo OutputFile { get; set; }

		/// <summary>
		/// Решение
		/// </summary>
		[XmlIgnore]
		public Solution ParentSolution { get; set; }

		/// <summary>
		/// Разрешенные файлы
		/// Кандидат к удалению
		/// </summary>
		protected HashSet<string> AllowedFiles = new HashSet<string>();

		/// <summary>
		/// Разрешенные расширения в проекте
		/// </summary>
		protected HashSet<string> AllowedExtension = new HashSet<string>();

		/// <summary>
		/// Номер (ID) задачи
		/// </summary>
		public int Number { get; set; }
		/// <summary>
		/// Сайт задачи
		/// </summary>
		public string Site { get; set; }
		/// <summary>
		/// Язык
		/// </summary>
		public string Lang { get; set; }
		/// <summary>
		/// Название задачи
		/// </summary>
		public string TaskName { get; set; }

		/// <summary>
		/// Директория решения
		/// </summary>
		[XmlIgnore]
		public string Path
		{
			get { return Dir.FullName; }
			set { Dir = new DirectoryInfo(value); }
		}

		/// <summary>
		/// Имя проекта (Имя задачи вместе с сайтом, номером и языком)
		/// </summary>
		public string Name
		{
			get { return String.Format("{0} {1:D4}. {2} [{3}]", Site, Number, TaskName, Lang); }
			set
			{
				if (Number == 0 || Site == "" || Lang == "" || TaskName == "")
				{
					throw new NotImplementedException();
				}
			}
		}

		/// <summary>
		/// Пространство имён по умолчанию (кандидат к переносу в другой класс)
		/// </summary>
		public string RootNamespace
		{
			get { return String.Format("{0}_{1:D4}", Site, Number); }
		}

		/// <summary>
		/// Имя кодового файла (кандидат на изменение публичности)
		/// </summary>
		public string CodeFileName
		{
			get { return String.Format("Task_{0}{1:D4}.{2}", Site, Number, Lang); }
		}

		protected Project()
		{
		}

		protected Project(string path)
		{
			Path = path;
		}

		/// <summary>
		/// Поиск файлов ввода-вывода и кода
		/// </summary>
		public void FindFiles()
		{
			FindProjectFiles();
			foreach (var i in Dir.GetFiles())
			{
				if (i.Name == MyEnum.Input)
				{
					InputFile = i;
					AllowedFiles.Add(i.FullName);
				}
				if (i.Name == MyEnum.Output)
				{
					OutputFile = i;
					AllowedFiles.Add(i.FullName);
				}
				if (i.Extension == MyEnum.Cpp || i.Extension == MyEnum.CSharp || i.Extension == MyEnum.Python)
				{
					CodeFile = i;
					AllowedFiles.Add(i.FullName);
				}
			}
		}

		/// <summary>
		/// Поиск файлов проекта
		/// </summary>
		protected abstract void FindProjectFiles();

		/// <summary>
		/// Очистка
		/// </summary>
		public void Clean()
		{
			FindFiles();
			foreach (var i in Dir.GetFiles())
			{
				if (!AllowedFiles.Contains(i.FullName))
				{
					Directory.CreateDirectory(ParentSolution.Dir + MyEnum.Trash + Name);
					Directory.Move(i.FullName, ParentSolution.Dir + MyEnum.Trash + Name + MyEnum.Slash + i.Name);
				}
			}
		}

		/// <summary>
		/// Кандидат на перенос или удаление или переименование
		/// </summary>
		public abstract void CreateFiles();

		/// <summary>
		/// Копирование из Templates в папку с проектом
		/// </summary>
		/// <param name="templatename">Имя шаблонного проекта</param>
		public void CreateFiles(string templatename)
		{
			if (Dir.Exists)
			{
				throw new Exception("Папка с проектом уже есть");
			}
			try
			{
				Solution.DirectoryCopy(@"Templates\" + templatename, Dir.FullName, true);
			}
			catch
			{
				// ignored
			}
		}
	}
}