/*
csc.exe /t:library StrConvHttpModule.cs /r:C:\windows\Microsoft.NET\Framework\v1.1.4322\Microsoft.VisualBasic.dll 

	<httpModules>
		<add name="StrConvHttpModule" type="Microshaoft.HttpModules.StrConvHttpModule, StrConvHttpModule" />
	</httpModules>
*/
namespace Microshaoft.HttpModules
{
	using System;
	using System.Web; 
	using System.Collections;
	using System.Windows.Forms;

	using Microshaoft.IO;
	

	public class ResponseHangHttpModule : IHttpModule
	{
		Form1 _form ;
		public string ModuleName
		{
			get
			{
				return "ResponseHangHttpModule";
			}
		}
		public static HttpResponse _response;
		public void Init(HttpApplication application)
		{
			application.BeginRequest += (new EventHandler(this.Application_BeginRequest));
		}
		
		private void Application_BeginRequest(object sender, EventArgs e)
		{
			HttpApplication application = (HttpApplication) sender;
			HttpContext context = application.Context;
			context.Response.Write("module<br>");
			context.Response.Filter = new StrConvFilterStream(context.Response.Filter);
///			_form = new Form1();
///			_form.Text = "yxyweb";
			context.Response.Write(_form.Handle.ToString()+ "<br>");
///			_form.Show();
			context.Response.Write(_form.Handle.ToString()+ "<br>");
///			_response = context.Response;
		}

		public void Dispose()
		{
		}
	}
}

namespace Microshaoft.IO
{
	using System;
	using System.IO;
	using System.Web;
	using System.Text;
	using System.Threading;
	using System.Globalization;

	//using Microsoft.VisualBasic;

	public class StrConvFilterStream : Stream
	{
		private Stream _sink;
		private long _position;

		public StrConvFilterStream(Stream sink)
		{
			this._sink = sink;
		}

		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		public override long Length
		{
			get
			{
				return 0;
			}
		}

		public override long Position
		{
			get
			{
				return this._position;
			}
		set
			{
				this._position = value;
			}
		}

		public override long Seek(long offset, SeekOrigin direction)
		{
			return this._sink.Seek(offset, direction);
		}

		public override void SetLength(long length)
		{
			this._sink.SetLength(length);
		}

		public override void Close()
		{
			this._sink.Close();
		}

		public override void Flush()
		{
			this._sink.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._sink.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
///			int i = 0;
///			byte[] data = new byte[10]{1,2,3,4,5,6,7,8,9,10};
///			while (i < 1024 * 1024 * 10)
///			{
///				Thread.Sleep(500);
				this._sink.Write(buffer, offset, count);
///				this._sink.Flush();
///			}
			
		}
	}
}

namespace Microshaoft
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    
    using System.Text;
    using System.Windows.Forms;

	
	using Microshaoft.HttpModules;
    public class Form1 : Form
    {
        public Form1()
        {
            //InitializeComponent();
        }
        public override bool PreProcessMessage(ref Message msg)
        {
            ResponseHangHttpModule._response.Write("insert .....");
            return base.PreProcessMessage(ref msg);
        }
    }
}