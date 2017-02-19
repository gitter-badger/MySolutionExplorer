﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace MySolutionExplorer
{
	[Serializable]
	[XmlRoot("Solution")]
	[XmlInclude(typeof(Project))]
	[XmlInclude(typeof(CppProject))]
	public class Solution : List<Project>
	{
		[XmlIgnore]
		public DirectoryInfo Dir;

		public Solution()
		{

		}

		public Solution(string path)
		{
			Dir = new DirectoryInfo(path);
		}

		public void FindProjects()
		{
			foreach (var i in Dir.GetDirectories())
			{
				if (i.Name.Contains("[cpp]"))
				{
					Add(new CppProject(i.FullName));
				}
			}
		}

		public new void Add(Project item)
		{
			base.Add(item);
			item.ParentSolution = this;
		}
	}
}
