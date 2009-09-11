using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;

namespace ERY.Xle.Serialization
{
	public class XleSerializationInfo
	{
		XmlDocument doc;
		Stack<XmlElement> nodes = new Stack<XmlElement>();

		public ITypeBinder Binder { get; internal set; }

		internal XleSerializationInfo()
		{
			doc = new XmlDocument();
		}
		internal XleSerializationInfo(XmlDocument doc)
		{
			this.doc = doc;
		}

		internal XmlDocument XmlDoc
		{
			get { return doc; }
		}

		public XmlElement CurrentNode
		{
			get
			{
				return nodes.Peek();
			}
		}

		void Serialize(IXleSerializable o)
		{
			o.WriteData(this);
		}

		void AddAttribute(XmlNode node, string name, string value)
		{
			XmlAttribute attrib = doc.CreateAttribute(name);
			attrib.Value = value;

			node.Attributes.Append(attrib);
		}

		internal void BeginSerialize(IXleSerializable objectGraph)
		{
			var root = doc.CreateElement("Root");

			AddAttribute(root, "type", objectGraph.GetType().ToString());

			doc.AppendChild(root);

			nodes.Push(root);
			Serialize(objectGraph);

			System.Diagnostics.Debug.Assert(nodes.Count == 1);
			nodes.Clear();
		}

		#region --- Writing methods ---

		public void Write(string name, string value)
		{
			if (value == null) value = "";

			WriteGeneric(name, value);
		}
		public void Write(string name, double value)
		{
			WriteGeneric(name, value);
		}
		public void Write(string name, float value)
		{
			WriteGeneric(name, value);
		}
		public void Write(string name, bool value)
		{
			WriteGeneric(name, value);
		}
		public void Write(string name, char value)
		{
			WriteGeneric(name, value);
		}
		public void Write(string name, short value)
		{
			WriteGeneric(name, value);
		}
		public void Write(string name, int value)
		{
			WriteGeneric(name, value);
		}
		public void Write(string name, long value)
		{
			WriteGeneric(name, value);
		}
		public void Write(string name, decimal value)
		{
			WriteGeneric(name, value);
		}
		public void Write(string name, int[] value)
		{
			byte[] array = new byte[value.Length * 4];

			unsafe
			{
				fixed (int* val = value)
				{
					Marshal.Copy((IntPtr)val, array, 0, array.Length);
				}
			}
			Write(name, array);

		}
		public void Write(string name, byte[] value)
		{
			string newValue = Convert.ToBase64String(value, Base64FormattingOptions.None);

			XmlElement el = WriteGeneric(name, newValue);
			AddAttribute(el, "array", "true");
			AddAttribute(el, "encoding", "Base64");
		}
		public void Write(string name, IXleSerializable value)
		{
			XmlElement element = CreateElement(name);
			AddAttribute(element, "type", value.GetType().ToString());

			nodes.Push(element);

			Serialize(value);

			nodes.Pop();
		}

		public void Write<T>(string name, T[] value) where T : IXleSerializable
		{
			Write(name, value.ToList());
		}
		public void Write<T>(string name, List<T> value) where T : IXleSerializable
		{
			Type[] args = value.GetType().GetGenericArguments();
			Type listType = args[0];

			XmlElement element = CreateElement(name);
			AddAttribute(element, "array", "true");
			AddAttribute(element, "type", listType.ToString());

			nodes.Push(element);

			for (int i = 0; i < value.Count; i++)
			{
				XmlElement item = doc.CreateElement("Item");
				CurrentNode.AppendChild(item);

				if (value[i].GetType() != listType)
					AddAttribute(item, "type", value[i].GetType().ToString());

				nodes.Push(item);
				Serialize(value[i]);
				nodes.Pop();
			}

			nodes.Pop();
		}

		private XmlElement WriteGeneric<T>(string name, T value)
		{
			XmlElement element = doc.CreateElement(name);

			element.InnerText = value.ToString();

			CurrentNode.AppendChild(element);

			return element;
		}

		private XmlElement CreateElement(string name)
		{
			XmlElement element = doc.CreateElement(name);

			for (int i = 0; i < CurrentNode.ChildNodes.Count; i++)
			{
				if (CurrentNode.ChildNodes[i].Name == name)
					throw new ArgumentException("The name " + name + " already exists.");
			}

			CurrentNode.AppendChild(element);

			return element;
		}


		#endregion
		#region --- Reading methods ---


		private Type GetType(string name)
		{
			return Binder.GetType(name);
		}



		public object ReadObject(string name)
		{
			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new ArgumentException("Node " + name + " not found.");

			try
			{
				nodes.Push(element);
				return DeserializeObject();
			}
			finally
			{
				nodes.Pop();
			}
		}
		public string ReadString(string name)
		{
			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new ArgumentException("Node " + name + " not found.");

			if (element.Attributes["encoding"] != null)
			{
				throw new Exception("Cannot decode encoded strings.");
			}

			return element.InnerText;
		}
		public bool ReadBoolean(string name)
		{
			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new ArgumentException("Node " + name + " not found.");

			return bool.Parse(element.InnerText);
		}
		public int ReadInt32(string name)
		{
			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new ArgumentException("Node " + name + " not found.");

			return int.Parse(element.InnerText);
		}
		public int ReadInt32(string name, int defaultValue)
		{
			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				return defaultValue;

			return int.Parse(element.InnerText);
		}
		public double ReadDouble(string name)
		{
			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new ArgumentException("Node " + name + " not found.");

			return double.Parse(element.InnerText);
		}

		public int[] ReadInt32Array(string name)
		{
			byte[] array = ReadByteArray(name);
			int[] result = new int[array.Length / 4];

			if (array.Length % 4 != 0)
				throw new InvalidOperationException("Encoded array is wrong size!");

			unsafe
			{
				fixed (byte* ar = array)
				{
					Marshal.Copy((IntPtr)ar, result, 0, result.Length);
				}
			}

			return result;
		}

		public Array ReadArray(string name)
		{
			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new ArgumentException("Node " + name + " not found.");

			if (element.Attributes["array"] == null || element.Attributes["array"].Value != "true")
				throw new InvalidOperationException("Element " + name + " is not an array.");
			if (element.Attributes["type"] == null)
				throw new InvalidOperationException("Element " + name + " does not have type information.");

			Type type = GetType(element.Attributes["type"].Value);
			Type listType = typeof(List<>).MakeGenericType(type);
			Type arrayType = type.MakeArrayType();
			System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(listType);

			for (int i = 0; i < element.ChildNodes.Count; i++)
			{
				XmlElement item = (XmlElement)element.ChildNodes[i];

				if (item.Name != "Item")
					throw new InvalidOperationException("Could not understand data.  Expected Item, found " + item.Name + ".");

				nodes.Push(item);

				object o = DeserializeObject(type);
				list.Add(o);

				nodes.Pop();
			}

			Array retval = (Array)Activator.CreateInstance(arrayType, list.Count);
			list.CopyTo(retval, 0);

			return retval;
		}


		public byte[] ReadByteArray(string name)
		{
			XmlElement element = (XmlElement)CurrentNode[name];

			if (element == null)
				throw new ArgumentException("Node " + name + " not found.");

			if (element.Attributes["array"] == null || element.Attributes["array"].Value != "true")
				throw new InvalidOperationException("Element " + name + " is not an array.");
			if (element.Attributes["encoding"] == null)
				throw new InvalidOperationException("Element " + name + " does not have encoding information.");

			if (element.Attributes["encoding"].Value == "Base64")
			{
				byte[] array = Convert.FromBase64String(element.InnerText);
				return array;
			}
			else
			{
				throw new InvalidOperationException("Unrecognized encoding " + element.Attributes["encoding"]);
			}

		}

		#endregion



		internal object BeginDeserialize()
		{
			XmlElement root = (XmlElement)doc.ChildNodes[0];

			if (root.Name != "Root")
				throw new System.IO.IOException("Could not understand stream.  Expected to find a Root element, but found " + root.Name + ".");

			nodes.Push(root);

			object retval = DeserializeObject();

			System.Diagnostics.Debug.Assert(nodes.Count == 1);
			nodes.Pop();

			return retval;
		}

		private object DeserializeObject()
		{
			return DeserializeObject(null);
		}
		private object DeserializeObject(Type defaultType)
		{
			XmlAttribute attrib = CurrentNode.Attributes["type"];
			Type type = defaultType;

			if (attrib == null && defaultType == null)
				throw new System.IO.IOException("Object lacks type information.");
			else if (attrib != null)
			{
				// load the type if it is not the default type.
				string typename = CurrentNode.Attributes["type"].Value;

				type = Binder.GetType(typename);
			}

			IXleSerializable obj;

			try
			{
				obj = (IXleSerializable)Activator.CreateInstance(type, true);
			}
			catch (MissingMethodException e)
			{
				throw new MissingMethodException("Type " + type.ToString() + " does not have a default constructor.", e);
			}

			obj.ReadData(this);

			return obj;
		}



	}
}
