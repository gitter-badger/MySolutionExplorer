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
	/// Проект C++
	/// </summary>
	[Serializable]
	public class CppProject : VSProject
	{
		/// <summary>
		/// Файл .vcxproj версии 2017
		/// </summary>
		[XmlIgnore]
		private XmlProjectFile VS2017ProjectFile
		{
			get { return XmlProjectFiles[0]; }
			set { XmlProjectFiles[0] = value; }
		}

		/// <summary>
		/// Файл .vcxproj версии 2010
		/// </summary>
		[XmlIgnore]
		private XmlProjectFile VS2010ProjectFile
		{
			get { return XmlProjectFiles[1]; }
			set { XmlProjectFiles[1] = value; }
		}

		/// <summary>
		/// Инициализация
		/// </summary>
		private void Init()
		{
			VS2017ProjectFile = new XmlProjectFile
			{
				Suff = MyEnum.VS2017,
				Parent = this,
				Extension = MyEnum.VCXProj
			};
			VS2010ProjectFile = new XmlProjectFile
			{
				Suff = MyEnum.VS2010,
				Parent = this,
				Extension = MyEnum.VCXProj
			};
		}

		public CppProject() : base(2)
		{
			Init();
		}

		public CppProject(string path) : base(path, 2)
		{
			Init();
		}

		/// <summary>
		/// Создание файлов из шаблона, переименовывание, изменение имён внутри
		/// </summary>
		public override void CreateFiles()
		{
			CreateFiles(MyEnum.TemplateCppProj);

			FindProjectFiles();

			CreateProjects();

			CodeFile = new FileInfo(Dir + MyEnum.Slash + MyEnum.TemplateCpp);
			CodeFile = Solution.RenameFile(CodeFile, CodeFileName);

			LoadProjects();
			ReformVSProjects();

			FindFiles();
			FindProjectFiles();
		}

		/// <summary>
		/// Действия при преобразовании шаблонного проекта
		/// </summary>
		/// <param name="proj">Xml-проект</param>
		protected override void ReformVSProjXml(XmlProjectFile proj)
		{
			ReformRootNamespace(proj.Xml);
			ReformCodeFileName(proj.Xml);
			proj.Xml.Save(proj.File.FullName);
		}
	}
}