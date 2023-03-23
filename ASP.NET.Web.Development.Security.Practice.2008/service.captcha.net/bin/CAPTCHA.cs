//CaptchaPlaceHolder.aspx.cs
namespace Microshaoft.WebSecurity
{
	using System;
	using System.Configuration;
	using System.Web;
	using System.Web.UI;
	using System.Text;
	using Microshaoft;
	public class CaptchaPlaceHolder : Page
	{
		public static string _UrlPrefix = ConfigurationManager.AppSettings["UrlPrefix"];
		public int _W = IntegerHelper.Int32Parse(ConfigurationManager.AppSettings["DefaultWidth"]);
		public int _H = IntegerHelper.Int32Parse(ConfigurationManager.AppSettings["DefaultHeight"]);
		public int _L = IntegerHelper.Int32Parse(ConfigurationManager.AppSettings["DefaultTextLength"]);
		public string _RequestRSAPublicKey;
		public string _ImgID = "ID_" + Guid.NewGuid().ToString().Replace("-", "_");

		public string _DivID = "ID_" + Guid.NewGuid().ToString().Replace("-", "_");
		public string _DivObjVar = "var_" + Guid.NewGuid().ToString().Replace("-", "_");
		public string _DivChildrenNodesObjVar = "var_" + Guid.NewGuid().ToString().Replace("-", "_");
		public string _DivChildNodeObjVar = "var_" + Guid.NewGuid().ToString().Replace("-", "_");
		public string _iVar = "var_" + Guid.NewGuid().ToString().Replace("-", "_");
		public string _FunctionName = "function_" + Guid.NewGuid().ToString().Replace("-", "_");
		public string _CallbackName;
		public string _RefreshCallName;
		public string _ClientID;
		public string _RSASignMode;
		public string _ScriptObjVar = "var_" + Guid.NewGuid().ToString().Replace("-", "_");
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!_UrlPrefix.EndsWith("/"))
			{
				_UrlPrefix += "/";
			}
			//AntiXss
			_CallbackName = Request.QueryString["callback"];
			_CallbackName = AntiXss.JavaScriptEncode(_CallbackName, false);
///						if (!RegexValidateHelper.IsValidNameOrID(_CallbackName))
///						{
///							//AntiXss
///							_CallbackName = "";
///						}
			_RefreshCallName = Request.QueryString["refresh"];
			_RefreshCallName = AntiXss.JavaScriptEncode(_RefreshCallName, false);
			///			if (!RegexValidateHelper.IsValidNameOrID(_RefreshCallName))
			///			{
			///				//AntiXss
			///				_RefreshCallName = "";
			///			}
			_RSASignMode = Request.QueryString["sign"];
			//_RSASignMode = AntiXss.JavaScriptEncode(_RSASignMode, false);
			///			if (!RegexValidateHelper.IsValidNameOrID(_RSASignMode))
			///			{
			///				//AntiXss
			///				_RSASignMode = "";
			///			}
			_ClientID = Request.QueryString["id"];
			_ClientID = AntiXss.JavaScriptEncode(_ClientID, false);
			///			if (!RegexValidateHelper.IsValidNameOrID(_ClientID))
			///			{
			///				//AntiXss
			///				_ClientID = "";
			///			}
			//_XSS = " <script>alert('1')</script>";
			//_FunctionName += ;
			string s = Request.QueryString["w"];
			if (StringHelper.IsValidString(s))
			{
				_W = IntegerHelper.Int32Parse(s);
			}
			s = Request.QueryString["h"];
			if (StringHelper.IsValidString(s))
			{
				_H = IntegerHelper.Int32Parse(s);
			}
			s = Request.QueryString["l"];
			if (StringHelper.IsValidString(s))
			{
				_L = IntegerHelper.Int32Parse(s);
			}
			s = Request.QueryString["rsa"];
			if (StringHelper.IsValidString(s))
			{
				byte[] buffer = Encoding.ASCII.GetBytes(s);
				_RequestRSAPublicKey = CryptoHelper.BytesArrayToHexString(buffer);
			}
		}
	}
}
//CaptchaGenerator.aspx.cs
namespace Microshaoft.WebSecurity
{
	using System;
	using System.Configuration;
	using System.Text;
	using System.Web;
	using System.Web.UI;
	using System.Security.Cryptography;
	using Microshaoft;
	public class CaptchaGenerator : Page
	{
		public static string _UrlPrefix = ConfigurationManager.AppSettings["UrlPrefix"];
		private static string _ResponseSharedTripleDESKey = ConfigurationManager.AppSettings["ResponseSharedTripleDESKey"];
		private static string _ResponseSharedTripleDESIV = ConfigurationManager.AppSettings["ResponseSharedTripleDESIV"];
		private static string _ResponseRSAPrivateKey = ConfigurationManager.AppSettings["ResponseRSAPrivateKey"];
		private static string _InternalTripleDESKey = ConfigurationManager.AppSettings["InternalTripleDESKey"];
		private static string _InternalTripleDESIV = ConfigurationManager.AppSettings["InternalTripleDESIV"];
		private static string _CharactersText = ConfigurationManager.AppSettings["CharactersText"];
		public int _W = IntegerHelper.Int32Parse(ConfigurationManager.AppSettings["DefaultWidth"]);
		public int _H = IntegerHelper.Int32Parse(ConfigurationManager.AppSettings["DefaultHeight"]);
		public int _L = IntegerHelper.Int32Parse(ConfigurationManager.AppSettings["DefaultTextLength"]);
		public string _RequestRSAPublicKey;
		public string _RSASignMode;
		public string _ResponseRSAPublicKey;
		public string _ResponseRSASignature;
		public string _ImgID;
		public string _ImgObjVar = "var_" + Guid.NewGuid().ToString().Replace("-", "_");
		public string _CallbackName;
		public string _ClientID;
		public string _OrignalVerifyCode;
		public string _EncryptedVerifyCode;
		public string _InternalTripleDESEncryptedVerifyCode;
		public string _EncryptedMode;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!_UrlPrefix.EndsWith("/"))
			{
				_UrlPrefix += "/";
			}
			//AntiXss
			_ImgID = Request.QueryString["imgid"];
			///			//AntiXss 方法1
			_ImgID = AntiXss.JavaScriptEncode(_ImgID, false);
			///			if (!RegexValidateHelper.IsValidNameOrID(_ImgID))
			///			{
			///				//AntiXss 方法2
			///				_ImgID = "";
			///			}
			_CallbackName = Request.QueryString["callback"];
			_CallbackName = AntiXss.JavaScriptEncode(_CallbackName, false);
			///			if (!RegexValidateHelper.IsValidNameOrID(_CallbackName))
			///			{
			///				//AntiXss
			///				_CallbackName = "";
			///			}
			_RequestRSAPublicKey = Request.QueryString["rsa"];
			_RequestRSAPublicKey = AntiXss.JavaScriptEncode(_RequestRSAPublicKey, false);
			///			if (!RegexValidateHelper.IsValidHexString(_RequestRSAPublicKey))
			///			{
			///				//AntiXss
			///				_RequestRSAPublicKey = "";
			///			}
			_RSASignMode = Request.QueryString["sign"];
			_RSASignMode = AntiXss.JavaScriptEncode(_RSASignMode, false);
			///			if (!RegexValidateHelper.IsValidNameOrID(_RSASignMode))
			///			{
			///				//AntiXss
			///				_RSASignMode = "";
			///			}
			_ClientID = Request.QueryString["id"];
			_ClientID = AntiXss.JavaScriptEncode(_ClientID, false);
			///			if (!RegexValidateHelper.IsValidNameOrID(_CallbackName))
			///			{
			///				//AntiXss
			///				_ClientID = "";
			///			}
			string s = Request.QueryString["w"];
			if (StringHelper.IsValidString(s))
			{
				_W = IntegerHelper.Int32Parse(s);
			}
			s = Request.QueryString["h"];
			if (StringHelper.IsValidString(s))
			{
				_H = IntegerHelper.Int32Parse(s);
			}
			s = Request.QueryString["l"];
			if (StringHelper.IsValidString(s))
			{
				_L = IntegerHelper.Int32Parse(s);
			}
			s = RandomHelper.GenerateText
									(
										_CharactersText,
										_L
									);
			_OrignalVerifyCode = string.Format
											(
												"{1}{0}{2}",
												"\t",
												s,
												DateTime.Now.ToString("yyyyMMddHHmmss")
											);
			byte[] buffer = Encoding.ASCII.GetBytes(_OrignalVerifyCode);
			Array.Reverse(buffer);
			byte[] bytes = CryptoHelper.TripleDESEncrypt
											(
												buffer,
												//Encoding.ASCII,
												CryptoHelper.HexStringToBytesArray(_InternalTripleDESKey),
												CryptoHelper.HexStringToBytesArray(_InternalTripleDESIV)
											);
			_InternalTripleDESEncryptedVerifyCode = CryptoHelper.BytesArrayToHexString(bytes);
			if (StringHelper.IsValidString(_RequestRSAPublicKey)) //之前已经判断
			{
				bytes = CryptoHelper.HexStringToBytesArray(_RequestRSAPublicKey);
				CspParameters csp = new CspParameters();
				//csp.Flags = CspProviderFlags.UseMachineKeyStore; //network service 不需要此句
				RSACryptoServiceProvider provider = new RSACryptoServiceProvider(csp);
				bytes = CryptoHelper.HexStringToBytesArray(_RequestRSAPublicKey);
				string requestRSAPublicKey = Encoding.ASCII.GetString(bytes);
				bytes = CryptoHelper.RSAEncrypt(provider, buffer, requestRSAPublicKey, true);
				_EncryptedVerifyCode = CryptoHelper.BytesArrayToHexString(bytes);
				_EncryptedMode = "RSA";
			}
			else
			{
				bytes = CryptoHelper.TripleDESEncrypt
												(
													buffer,
													CryptoHelper.HexStringToBytesArray(_ResponseSharedTripleDESKey),
													CryptoHelper.HexStringToBytesArray(_ResponseSharedTripleDESIV)
												);
				_EncryptedVerifyCode = CryptoHelper.BytesArrayToHexString(bytes);
				_EncryptedMode = "3DES";
			}
			if (StringHelper.IsValidString(_RSASignMode))
			{
				CspParameters csp = new CspParameters();
				//csp.Flags = CspProviderFlags.UseMachineKeyStore; //network service 不需要此句
				RSACryptoServiceProvider provider = new RSACryptoServiceProvider(csp);
				//rsa privateKey
				provider.FromXmlString(_ResponseRSAPrivateKey);
				string key = provider.ToXmlString(true);
				if (_RSASignMode.ToLower() == "md5")
				{
					//signature rsa sha1
					bytes = CryptoHelper.RSASignMD5(provider, buffer, key);
				}
				else if (_RSASignMode.ToLower() == "sha1")
				{
					//signature rsa md5
					bytes = CryptoHelper.RSASignSHA1(provider, buffer, key);
				}
				_ResponseRSASignature = CryptoHelper.BytesArrayToHexString(bytes);
				//rsa publicKey
				key = provider.ToXmlString(false);
				//bytes = Encoding.ASCII.GetBytes(key);
				_ResponseRSAPublicKey = key;
			}
		}
	}
}
//Captcha.aspx.cs
namespace Microshaoft.WebSecurity
{
	using System;
	using System.Configuration;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;
	using System.Text;
	using System.Web.UI;
	using Microshaoft;
	public class Captcha : Page
	{
		private static string _InternalTripleDESKey = ConfigurationManager.AppSettings["InternalTripleDESKey"];
		private static string _InternalTripleDESIV = ConfigurationManager.AppSettings["InternalTripleDESIV"];
		public int _W = IntegerHelper.Int32Parse(ConfigurationManager.AppSettings["DefaultWidth"]);
		public int _H = IntegerHelper.Int32Parse(ConfigurationManager.AppSettings["DefaultHeight"]);
		protected void Page_Load(object sender, EventArgs e)
		{
			//return;
			string text = Request.QueryString["text"];
			byte[] buffer = null;
			try
			{
				buffer = CryptoHelper.TripleDESDecrypt
												(
													CryptoHelper.HexStringToBytesArray(text),
													CryptoHelper.HexStringToBytesArray(_InternalTripleDESKey),
													CryptoHelper.HexStringToBytesArray(_InternalTripleDESIV)
												);
			}
			catch// (Exception exception)
			{
				buffer = new byte[0];
			}
			Array.Reverse(buffer);
			text = Encoding.ASCII.GetString(buffer);
			string[] a = text.Split(new char[] { '\t' });
			text = a[0];
			string s = Request.QueryString["w"];
			if (StringHelper.IsValidString(s))
			{
				_W = IntegerHelper.Int32Parse(s);
			}
			s = Request.QueryString["h"];
			if (StringHelper.IsValidString(s))
			{
				_H = IntegerHelper.Int32Parse(s);
			}
			Bitmap bmp = new Bitmap(_W, _H);
			//GraphicsHelper.DrawLinearGradientBackground(bmp);
			GraphicsHelper.DrawRandomColorsNoiseLines
												(
													bmp,
													1
												);
			GraphicsHelper.DrawRandomColorsNoisePoints
												(
													bmp,
													1
												);
			GraphicsHelper.DrawTextString
									(
										bmp,
										text,
				///										new Color[]
				///												{
				///													Color.Black,
				///													Color.Red,
				///													Color.Green,
				///													Color.Blue,
				///												},
										new string[]
												{
													"System",
													"Courier New",
													"Fixedsys",
													"Arial",
//													"Arial Black",
												}
									);
			GraphicsHelper.RandomDistortImage(bmp, 5);
			GraphicsHelper.DrawRandomColorsEdgeLine
												(
													bmp,
													1
												);
			MemoryStream ms = new MemoryStream();
			bmp.Save(ms, ImageFormat.Png);
			Response.ClearContent();
			//Response.ContentType = "image/Png";
			Response.BinaryWrite(ms.ToArray());
			bmp.Dispose();
			bmp = null;
		}
	}
}
//CryptoHelper.cs
namespace Microshaoft
{
	using System;
	using System.Security.Cryptography;
	using System.Text;
	using System.IO;
	public static class CryptoHelper
	{
		public static byte[] RSASignSHA1
								(
									byte[] data
									, string privateKey
								)
		{
			RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
			return RSASignSHA1
						(
							provider
							, data
							, privateKey
						);
		}
		public static byte[] RSASignSHA1
				(
					RSACryptoServiceProvider provider
					, byte[] data
					, string privateKey
				)
		{
			provider.FromXmlString(privateKey);
			return provider.SignHash
								(
									ComputeSHA1(data)
									, "SHA1"
								);
		}
		public static bool RSAVerifySHA1
								(
									byte[] data
									, string publicKey
									, byte[] signature
								)
		{
			RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
			return RSAVerifySHA1
							(
								provider
								, data
								, publicKey
								, signature
							);
		}
		public static bool RSAVerifySHA1
								(
									RSACryptoServiceProvider provider
									, byte[] data
									, string publicKey
									, byte[] signature
								)
		{
			provider.FromXmlString(publicKey);
			return provider.VerifyHash
								(
									ComputeSHA1(data)
									, "SHA1"
									, signature
								);
		}
		public static byte[] RSASignMD5
								(
									byte[] data
									, string privateKey
								)
		{
			RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
			return RSASignMD5
						(
							provider
							, data
							, privateKey
						);
		}
		public static byte[] RSASignMD5
								(
									RSACryptoServiceProvider provider
									, byte[] data
									, string privateKey
								)
		{
			provider.FromXmlString(privateKey);
			return provider.SignHash
						(
							ComputeMD5(data)
							, "MD5"
						);
		}
		public static bool RSAVerifyMD5
								(
									byte[] data
									, string publicKey
									, byte[] signature
								)
		{
			RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
			return RSAVerifyMD5
								(
									provider
									, data
									, publicKey
									, signature
								);
		}
		public static bool RSAVerifyMD5
								(
									RSACryptoServiceProvider provider
									, byte[] data
									, string publicKey
									, byte[] signature
								)
		{
			provider.FromXmlString(publicKey);
			return provider.VerifyHash
								(
									ComputeMD5(data)
									, "MD5"
									, signature
								);
		}
		public static byte[] RSAEncrypt
								(
									byte[] data
									, string publicKey
									, bool DoOAEPPadding
								)
		{
			RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
			return RSAEncrypt
						(
							provider,
							data,
							publicKey,
							DoOAEPPadding
						);
		}
		public static byte[] RSAEncrypt
						(
							RSACryptoServiceProvider provider
							, byte[] data
							, string publicKey
							, bool DoOAEPPadding
						)
		{
			provider.FromXmlString(publicKey);
			return provider.Encrypt(data, DoOAEPPadding);
		}
		public static byte[] RSADecrypt
								(
									byte[] data
									, string privateKey
									, bool DoOAEPPadding
								)
		{
			RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
			return RSADecrypt
						(
							provider,
							data,
							privateKey,
							DoOAEPPadding
						);
		}
		public static byte[] RSADecrypt
						(
							RSACryptoServiceProvider provider
							, byte[] data
							, string privateKey
							, bool DoOAEPPadding
						)
		{
			provider.FromXmlString(privateKey);
			return provider.Decrypt(data, DoOAEPPadding);
		}
		public static byte[] TripleDESDecrypt
										(
											byte[] data
											, byte[] Key
											, byte[] IV
										)
		{
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			des.Key = Key;
			des.IV = IV;
			return des.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
		}
		public static byte[] TripleDESDecrypt
										(
											string text
											, string HexStringKey
											, string HexStringIV
										)
		{
			return TripleDESDecrypt
							(
								HexStringToBytesArray(text)
								, HexStringToBytesArray(HexStringKey)
								, HexStringToBytesArray(HexStringIV)
							);
		}
		public static byte[] TripleDESDecrypt
									(
										string text
										, byte[] Key
										, byte[] IV
									)
		{
			return TripleDESDecrypt
							(
								HexStringToBytesArray(text)
								, Key
								, IV
							);
		}
		public static string TripleDESDecrypt
									(
										string text
										, string HexStringKey
										, string HexStringIV
										, Encoding e //原文的encoding
									)
		{
			return e.GetString
				(
					TripleDESDecrypt
						(
							text
							, HexStringKey
							, HexStringIV
						)
				);
		}
		public static string TripleDESDecrypt
									(
										string text
										, byte[] Key
										, byte[] IV
										, Encoding e //原文的encoding
									)
		{
			return e.GetString
						(
							TripleDESDecrypt
								(
									text
									, Key
									, IV
								)
						);
		}
		public static string GenerateTripleDESHexStringKey()
		{
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			des.GenerateKey();
			return BytesArrayToHexString(des.Key);
		}
		public static string GenerateTripleDESHexStringIV()
		{
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			des.GenerateIV();
			return BytesArrayToHexString(des.IV);
		}
		public static byte[] TripleDESEncrypt
										(
											byte[] data
											, byte[] Key
											, byte[] IV
										)
		{
			TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
			des.Key = Key;
			des.IV = IV;
			return des.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
		}
		public static byte[] TripleDESEncrypt
										(
											string text
											, Encoding e
											, byte[] Key
											, byte[] IV
										)
		{
			return TripleDESEncrypt
							(
								e.GetBytes(text)
								, Key
								, IV
							);
		}
		public static byte[] TripleDESEncrypt
										(
											string text
											, Encoding e
											, string HexStringKey
											, string HexStringIV
										)
		{
			return TripleDESEncrypt
							(
								text
								, e
								, HexStringToBytesArray(HexStringKey)
								, HexStringToBytesArray(HexStringIV)
							);
		}
		public static byte[] ComputeSHA1(byte[] data)
		{
			return new SHA1CryptoServiceProvider().ComputeHash(data);
		}
		public static byte[] ComputeSHA1(string text, Encoding e)
		{
			return ComputeSHA1(e.GetBytes(text));
		}
		public static byte[] ComputeSHA1(string text)
		{
			return ComputeSHA1(text, Encoding.UTF8);
		}
		public static byte[] ComputeSHA1(Stream stream)
		{
			return new SHA1CryptoServiceProvider().ComputeHash(stream);
		}
		public static byte[] ComputeMD5(byte[] data)
		{
			return new MD5CryptoServiceProvider().ComputeHash(data, 0, data.Length);
		}
		public static byte[] ComputeMD5(string text, Encoding e)
		{
			return ComputeMD5(e.GetBytes(text));
		}
		public static byte[] ComputeMD5(string text)
		{
			return ComputeMD5(text, Encoding.UTF8);
		}
		public static byte[] ComputeMD5(Stream stream)
		{
			return new MD5CryptoServiceProvider().ComputeHash(stream);
		}
		public static string BytesArrayToHexString(byte[] data)
		{
			return BitConverter.ToString(data).Replace("-", "");
		}
		public static byte[] HexStringToBytesArray(string text)
		{
			text = text.Replace(" ", "");
			int l = text.Length;
			byte[] buffer = new byte[l / 2];
			for (int i = 0; i < l; i += 2)
			{
				buffer[i / 2] = Convert.ToByte(text.Substring(i, 2), 16);
			}
			return buffer;
		}
	}
}
//DateTimeHelper.cs
namespace Microshaoft
{
	using System;
	using System.Globalization;
	public static class DateTimeHelper
	{
		public static bool IsVaildateTimestamp(DateTime timeStamp, int timeoutSeconds)
		{
			long l = SecondsDiffNow(timeStamp);
			return ((l > 0) && (l < timeoutSeconds));
		}
		public static long MillisecondsDiffNow(DateTime time)
		{
			long now = DateTime.Now.Ticks;
			long t = time.Ticks;
			return (now - t) / 10000;
		}
		public static long SecondsDiffNow(DateTime time)
		{
			return MillisecondsDiffNow(time) / 1000;
		}
		public static string Get_MMddHHmmss_String(DateTime time)
		{
			return time.ToString("MMddHHmmss");
		}
		public static string Get_yyyyMMddHHmmss_String(DateTime time)
		{
			return time.ToString("yyyyMMddHHmmss");
		}
		public static string Get_yyyyMMdd_String(DateTime time)
		{
			return time.ToString("yyyyMMdd");
		}
		public static DateTime Parse_yyyyMMddHHmmss(string text)
		{
			DateTime time = DateTime.TryParseExact
											(
												text
												, "yyyyMMddHHmmss"
												, DateTimeFormatInfo.InvariantInfo
												, DateTimeStyles.None
												, out time
											) ? time : DateTime.MinValue;
			return time;
		}
		public static DateTime Parse_MMddHHmmss(int year, string text)
		{
			return Parse_yyyyMMddHHmmss(year.ToString("0000") + text);
		}
	}
}
//IntegerHelper.cs
namespace Microshaoft
{
	using System;
	public static class IntegerHelper
	{
		public static int Int32Parse(string text)
		{
			int i = int.TryParse
							(
								text
								, out i
							) ? i : int.MinValue;
			return i;
		}
		public static uint UInt32Parse(string text)
		{
			uint i = uint.TryParse
								(
									text
									, out i
								) ? i : uint.MaxValue;
			return i;
		}
		public static short Int16Parse(string text)
		{
			short i = short.TryParse
								(
									text
									, out i
								) ? i : short.MinValue;
			return i;
		}
		public static ushort UInt16Parse(string text)
		{
			ushort i = ushort.TryParse
									(
										text
										, out i
									) ? i : ushort.MaxValue;
			return i;
		}
		public static long Int64Parse(string text)
		{
			long i = long.TryParse
								(
									text
									, out i
								) ? i : long.MinValue;
			return i;
		}
		public static ulong UInt64Parse(string text)
		{
			ulong i = ulong.TryParse
								(
									text
									, out i
								) ? i : ulong.MaxValue;
			return i;
		}
	}
}
//StringHelper.cs
namespace Microshaoft
{
	using System;
	public static class StringHelper
	{
		public static bool IsValidString(string text)
		{
			return
				(
					!string.IsNullOrEmpty(text) &&
				///					text != string.Empty &&
				///					text != null &&
					text.Trim().Length > 0
				);
		}
	}
}
//RandomHelper.cs
namespace Microshaoft
{
	using System;
	public class RandomHelper
	{
		public static string GenerateText
								(
									char[] characters
									, int length
								)
		{
			Random random = new Random();
			string s = "";
			//生成验证码字符串
			for (int i = 0; i < length; i++)
			{
				s += characters[random.Next(characters.Length)];
			}
			return s;
		}
		public static string GenerateText
									(
										string charactersText
										, int length
									)
		{
			return GenerateText
							(
								charactersText.ToCharArray(),
								length
							);
		}
	}
}
//GraphicsHelper.cs
namespace Microshaoft
{
	using System;
	using System.Drawing;
	using System.Drawing.Text;
	using System.Drawing.Imaging;
	using System.Drawing.Drawing2D;
	public static class GraphicsHelper
	{
		private struct FontState
		{
			public Font TextFont;
			public float TextWidth;
			public float TextHeight;
		}
		public static void DrawLinearGradientBackground
										(
											Bitmap bmp
										)
		{
			Random random = new Random();
			using (Graphics g = Graphics.FromImage(bmp))
			{
				using (
						Brush b = new LinearGradientBrush
												(
													new Rectangle
																(
																	0,
																	0,
																	bmp.Width,
																	bmp.Height
																),
													Color.FromArgb
																(
																	random.Next(256),
																	random.Next(256),
																	random.Next(256)
																),
													//Color.Green,
													Color.FromArgb
																(
																	random.Next(256),
																	random.Next(256),
																	random.Next(256)
																),
													//Color.Yellow,
													(float) (random.Next() * 360),
													false
												)
						)
				{
					g.FillRectangle(b, 0, 0, bmp.Width, bmp.Height);
				}
			}
		}
		public static void DrawTextString
						(
							Bitmap bmp,
							string text,
			//							Color[] foregroundColors,
							string[] fontsNames
						)
		{
			Random random = new Random();
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
				int l = text.Length;
				float w = (float) bmp.Width / l;
				float h = (float) bmp.Height;
				float p = 0f;
				FontState[] fontStates = new FontState[l];
				for (int i = 0; i < l; i++)
				{
					FontStyle fs = FontStyle.Bold;
					if (random.Next() % 2 == 0)
					{
						fs |= FontStyle.Italic;
					}
					float fontSize = (w < h ? w : h);
					Font font;
					SizeF sizeF;
					do
					{
						font = new Font
									(
										fontsNames[random.Next(fontsNames.Length)],
										fontSize,
										fs
									);
						fontSize --;
						sizeF = g.MeasureString(text[i].ToString(), font);
					} while ((sizeF.Width > w) || (sizeF.Height > h));
					FontState state = new FontState();
					state.TextFont = font;
					state.TextWidth = (sizeF.Width);
					state.TextHeight = (sizeF.Height);
					fontStates[i] = state;
					p += sizeF.Width;
				}
				p = (bmp.Width - p) / 2.0f;
				for (int i = 0; i < fontStates.Length; i++)
				{
//					Color color = ;
					using (Brush brush = new LinearGradientBrush
														(
															new Rectangle
																	(
																		0,
																		0,
																		(int) fontStates[i].TextWidth / 2,
																		(int) fontStates[i].TextHeight / 2
																	),
															//Color.Black,
															Color.FromArgb
																		(
																			random.Next(256),
																			random.Next(256),
																			random.Next(256)
																		),
															//Color.Blue,
															Color.FromArgb
																		(
																			random.Next(256),
																			random.Next(256),
																			random.Next(256)
																		),
															(float) (random.Next() * 360),
															false
														)
						)
					{
						Brush b =  new SolidBrush(Color.Black);
///												(
///													Color.FromArgb
///																(
///																	random.Next(256),
///																	random.Next(256),
///																	random.Next(256)
///																)
///												);
						g.DrawString
						(
							text[i].ToString()
							, fontStates[i].TextFont
							, b
							, (float) p - 1
							, (bmp.Height - fontStates[i].TextHeight) / 2.0f - 1
						);
						g.DrawString
						(
							text[i].ToString()
							, fontStates[i].TextFont
							, b
							, (float) p + 1
							, (bmp.Height - fontStates[i].TextHeight) / 2.0f + 1
						);
///						GraphicsPath gp = new GraphicsPath();
///						gp.AddString
///								(
///									text[i].ToString(),
///									fontStates[i].TextFont.FontFamily,
///									(int) FontStyle.Bold,
///									fontStates[i].TextFont.Size,
///									new PointF(p, (bmp.Height - fontStates[i].TextHeight) / 2.0f),
///									StringFormat.GenericDefault
///								);
///					   g.DrawPath(new Pen(Color.Red, 1), gp);
						g.DrawString
						(
							text[i].ToString()
							, fontStates[i].TextFont
							, brush
							, (float) p
							, (bmp.Height - fontStates[i].TextHeight) / 2.0f
						);
					}
					p += fontStates[i].TextWidth;
				}
			}
		}
		public static void RandomDistortImage(Bitmap bmp, int i)
		{
			Random random = new Random();
			double d = i * (random.Next(2) == 1 ? 1 : -1);
			DistortImage(bmp, d);
		}
		public static void DistortImage
								(
									Bitmap bmp,
									double distortion
								)
		{
			int width = bmp.Width;
			int height = bmp.Height;
			using (Bitmap copy = (Bitmap)bmp.Clone())
			{
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						int newX = (int)(x + (distortion * Math.Sin(Math.PI * y / 64)));
						int newY = (int)(y + (distortion * Math.Cos(Math.PI * x / 64)));
						if (newX < 0 || newX >= width)
						{
							newX = 0;
						}
						if (newY < 0 || newY >= height)
						{
							newY = 0;
						}
						bmp.SetPixel(x, y, copy.GetPixel(newX, newY));
					}
				}
			}
		}
		public static Bitmap Clear
								(
									Bitmap bmp,
									Color color
								)
		{
			Graphics g = Graphics.FromImage(bmp);
			g.Clear(color);
			g.Dispose();
			g = null;
			return bmp;
		}
		public static Bitmap DrawRandomColorsNoiseLines
												(
													Bitmap bmp,
													int lineWidth
												)
		{
			Random random = new Random();
			Graphics g = Graphics.FromImage(bmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			//画噪线
			int area = bmp.Width * bmp.Height;
			for (int i = 0; i < area / 500; i++)
			{
				int x1 = random.Next(bmp.Width);
				int y1 = random.Next(bmp.Height);
				int x2 = random.Next(bmp.Width);
				int y2 = random.Next(bmp.Height);
				g.DrawLine
						(
							new Pen
									(
										Color.FromArgb
													(
														random.Next(256),
														random.Next(256),
														random.Next(256)
													),
										lineWidth
									),
							x1,
							y1,
							x2,
							y2
						);
			}
			g.Dispose();
			g = null;
			return bmp;
		}
		public static void DrawRandomColorsNoisePoints
												(
													Bitmap bmp,
													int brushWidth
												)
		{
			Random random = new Random();
			Graphics g = Graphics.FromImage(bmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			int area = bmp.Width * bmp.Height;
			//画噪点
			for (int i = 0; i < area / 20; i++)
			{
				int x = random.Next(bmp.Width);
				int y = random.Next(bmp.Height);
				bmp.SetPixel
						(
							x,
							y,
							Color.FromArgb
										(
											random.Next(256),
											random.Next(256),
											random.Next(256)
										)
						);
			}
			g.Dispose();
			g = null;
		}
		public static void DrawRandomColorsEdgeLine
											(
												Bitmap bmp,
												int brushWidth
											)
		{
			Random random = new Random();
			Graphics g = Graphics.FromImage(bmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			//画边框
			g.DrawLine
					(
						new Pen
								(
									Color.FromArgb
												(
													random.Next(256),
													random.Next(256),
													random.Next(256)
												),
									brushWidth
								),
						0,
						0,
						bmp.Width - 1,
						0
					);
			g.DrawLine
					(
						new Pen
								(
									Color.FromArgb
												(
													random.Next(256),
													random.Next(256),
													random.Next(256)
												),
									brushWidth
								),
						0,
						0,
						0,
						bmp.Height - 1
					);
			g.DrawLine
					(
						new Pen
								(
									Color.FromArgb
												(
													random.Next(256),
													random.Next(256),
													random.Next(256)
												),
									brushWidth
								),
						bmp.Width - 1,
						0,
						bmp.Width - 1,
						bmp.Height - 1
					);
			g.DrawLine
					(
						new Pen
								(
									Color.FromArgb
												(
													random.Next(256),
													random.Next(256),
													random.Next(256)
												),
									brushWidth
								),
						0,
						bmp.Height - 1,
						bmp.Width - 1,
						bmp.Height - 1
					);
			g.Dispose();
			g = null;
		}
	}
}
//RegexValidateHelper.cs
namespace Microshaoft
{
	//using System;
	using System.Text.RegularExpressions;
	public static class RegexValidateHelper
	{
		public static bool IsValidNameOrID(string s)
		{
			return StringHelper.IsValidString(s) && Regex.IsMatch(s, @"^[_A-Za-z]\w+$");
		}
		public static bool IsValidHexString(string s)
		{
			return StringHelper.IsValidString(s) && Regex.IsMatch(s, @"[0-9A-Fa-f]");
		}
		public static bool IsValidEmail(string s)
		{
			return Regex.IsMatch(s, @"([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
		}
		public static bool IsValidDate(string s)
		{
			return Regex.IsMatch(s, @"((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
		}
		public static bool IsValidDateTime(string s)
		{
			return Regex.IsMatch(s, @"((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
		}
	}
}
namespace Microshaoft //Microsoft.Security.Application
{
	#region Namespaces
	using System;
	using System.Drawing;
	using System.Web;
	using System.Text;
	#endregion
	#region Namespace - Microsoft.Security.Application
	public class AntiXss
	{
		#region MEMBERS
		///---------------------------------------------------------------------
		/// <summary>
		///	 Empty string for Visual Basic Script contextes
		/// </summary>
		///---------------------------------------------------------------------
		private const string EmptyStringVBS = "\"\"";
		///---------------------------------------------------------------------
		/// <summary>
		///	 Empty string for Java Script contextes
		/// </summary>
		///---------------------------------------------------------------------
		private const string EmptyStringJavaScript = "''";
		#region WHITELIST_CHAR_ARRAY_INITIALIZATION
		/// <summary>
		/// Initializes character Html encoding array
		/// </summary>
		private static char[][] WhitelistCodes = InitWhitelistCodes();
		private static char[][] InitWhitelistCodes()
		{
			char[][] allCharacters = new char[65536][];
			char[] thisChar;
			for (int i = 0; i < allCharacters.Length; i++)
			{
				if (
					(i >= 97 && i <= 122) ||		// a-z
					(i >= 65 && i <= 90) ||		 // A-Z
					(i >= 48 && i <= 57) ||		 // 0-9
					i == 32 ||					  // space
					i == 46 ||					  // .
					i == 44 ||					  // ,
					i == 45 ||					  // -
					i == 95 ||					  // _
					(i >= 256 && i <= 591) ||	   // Latin,Extended-A,Latin Extended-B
					(i >= 880 && i <= 2047) ||	  // Greek and Coptic,Cyrillic,Cyrillic Supplement,Armenian,Hebrew,Arabic,Syriac,Arabic,Supplement,Thaana,NKo
					(i >= 2304 && i <= 6319) ||	 // Devanagari,Bengali,Gurmukhi,Gujarati,Oriya,Tamil,Telugu,Kannada,Malayalam,Sinhala,Thai,Lao,Tibetan,Myanmar,eorgian,Hangul Jamo,Ethiopic,Ethiopic Supplement,Cherokee,Unified Canadian Aboriginal Syllabics,Ogham,Runic,Tagalog,Hanunoo,Buhid,Tagbanwa,Khmer,Mongolian
					(i >= 6400 && i <= 6687) ||	 // Limbu, Tai Le, New Tai Lue, Khmer, Symbols, Buginese
					(i >= 6912 && i <= 7039) ||	 // Balinese
					(i >= 7680 && i <= 8191) ||	 // Latin Extended Additional, Greek Extended
					(i >= 11264 && i <= 11743) ||   // Glagolitic, Latin Extended-C, Coptic, Georgian Supplement, Tifinagh, Ethiopic Extended
					(i >= 12352 && i <= 12591) ||   // Hiragana, Katakana, Bopomofo
					(i >= 12688 && i <= 12735) ||   // Kanbun, Bopomofo Extended
					(i >= 12784 && i <= 12799) ||   // Katakana, Phonetic Extensions
					(i >= 40960 && i <= 42191) ||   // Yi Syllables, Yi Radicals
					(i >= 42784 && i <= 43055) ||   // Latin Extended-D, Syloti, Nagri
					(i >= 43072 && i <= 43135) ||   // Phags-pa
					(i >= 44032 && i <= 55215) ||   // Hangul Syllables
					(i >= 19968 && i <= 40899)	  // Mixed japanese/chinese/korean
				)
				{
					allCharacters[i] = null;
				}
				else
				{
					string iString = i.ToString();
					int iStringLen = iString.Length;
					thisChar = new char[iStringLen];	 // everything else
					for (int j = 0; j < iStringLen; j++)
					{
						thisChar[j] = iString[j];
					}
					allCharacters[i] = thisChar;
				}
			}
			return allCharacters;
		}
		#endregion
		#endregion
		#region Encoding Methods
		#region HTMLEncode - string input
		///---------------------------------------------------------------------
		/// <summary>
		/// Encodes input strings for use in HTML.
		/// </summary>
		/// <param name="input">String to be encoded</param>
		/// <returns>
		///	 Encoded string for use in HTML.
		/// </returns>
		/// <remarks>
		/// This function will encode all but known safe characters.  Encoded characters are encoded using the &amp;#DECIMAL; notation.
		/// <newpara/>
		/// Safe characters include:
		/// <list type="table">
		/// <item><term>a-z</term><description>Lower case alphabet</description></item>
		/// <item><term>A-Z</term><description>Upper case alphabet</description></item>
		/// <item><term>0-9</term><description>Numbers</description></item>
		/// <item><term>,</term><description>Comma</description></item>
		/// <item><term>.</term><description>Period</description></item>
		/// <item><term>-</term><description>Dash</description></item>
		/// <item><term>_</term><description>Underscore</description></item>
		/// <item><term> </term><description>Space</description></item>
		/// <item><term> </term><description>Other International character ranges</description></item>
		/// </list>
		/// <newpara/>
		/// Example inputs and encoded outputs:
		/// <list type="table">
		/// <item><term>alert('XSS Attack!');</term><description>alert&#40;&#39;XSS Attack&#33;&#39;&#41;&#59;</description></item>
		/// <item><term>user@contoso.com</term><description>user&#64;contoso.com</description></item>
		/// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross Site Scripting Library</description></item>
		/// </list></remarks>
		///---------------------------------------------------------------------
		public static string HtmlEncode(string input)
		{
			if (String.IsNullOrEmpty(input))
			{
				return string.Empty;
			}
			// Use a new char array.
			int len = 0;
			int tLen = input.Length;
			char[] returnMe = new char[tLen * 8];
			char[] thisChar;
			int thisCharID;
			for (int i = 0; i < tLen; i++)
			{
				thisCharID = (int)input[i];
				if (WhitelistCodes[thisCharID] != null)
				{
					// character needs to be encoded
					thisChar = WhitelistCodes[thisCharID];
					returnMe[len++] = '&';
					returnMe[len++] = '#';
					for (int j = 0; j < thisChar.Length; j++)
					{
						returnMe[len++] = thisChar[j];
					}
					returnMe[len++] = ';';
				}
				else
				{
					// character does not need encoding
					returnMe[len++] = input[i];
				}
			}
			return new String(returnMe, 0, len);
		}
		#endregion
		#region HTMLEncode - string input, KnownColor clr
		///---------------------------------------------------------------------
		/// <summary>
		/// Encodes input string and embeds in a SPAN tag for use in HTML.
		/// </summary>
		/// <param name="input">String to be encoded</param>
		/// <param name="clr">KnownColor like System.Drawing.KnownColor.CadetBlue</param>
		/// <returns>
		///	 Encoded string embebded within SPAN tag and style settings for use in HTML.
		/// </returns>
		/// <remarks>
		/// This function will encode all but known safe characters.  Encoded characters are encoded using the &amp;#DECIMAL; notation.
		/// <newpara/>
		/// Safe characters include:
		/// <list type="table">
		/// <item><term>a-z</term><description>Lower case alphabet</description></item>
		/// <item><term>A-Z</term><description>Upper case alphabet</description></item>
		/// <item><term>0-9</term><description>Numbers</description></item>
		/// <item><term>,</term><description>Comma</description></item>
		/// <item><term>.</term><description>Period</description></item>
		/// <item><term>-</term><description>Dash</description></item>
		/// <item><term>_</term><description>Underscore</description></item>
		/// <item><term> </term><description>Space</description></item>
		/// <item><term> </term><description>Other International character ranges</description></item>
		/// </list>
		/// <newpara/>
		/// Example inputs and encoded outputs:
		/// <list type="table">
		/// <item><term>alert('XSS Attack!');</term><description><div style='background-color : #ffffff'>alert&#40;&#39;XSS Attack&#33;&#39;&#41;&#59;</div></description></item>
		/// <item><term>user@contoso.com</term><description>user&#64;contoso.com</description></item>
		/// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross Site Scripting Library</description></item>
		/// </list></remarks>
		///---------------------------------------------------------------------
		public static string HtmlEncode(string input, KnownColor clr)
		{
			//HTMLEncode will handle the encoding
			// This check is for making sure that bgcolor is required or not.
			if (HttpContext.Current.Request.QueryString["MarkAntiXssOutput"] != null)
			{
				string returnInput = "<span name='#markantixssoutput' style ='background-color : " + Color.FromKnownColor(clr).Name + "'>" + HtmlEncode(input) + "</span>";
				return returnInput;
			}
			else
			{
				return HtmlEncode(input);
			}
		}
		#endregion
		#region HTMLAttributeEncode_Method
		///---------------------------------------------------------------------
		/// <summary>
		/// Encodes input strings for use in HTML attributes.
		/// </summary>
		/// <param name="input">String to be encoded</param>
		/// <returns>
		///	 Encoded string for use in HTML attributes.
		/// </returns>
		/// <remarks>
		/// This function will encode all but known safe characters.  Encoded characters are encoded using the &amp;#DECIMAL; notation.
		/// <newpara/>
		/// Safe characters include:
		/// <list type="table">
		/// <item><term>a-z</term><description>Lower case alphabet</description></item>
		/// <item><term>A-Z</term><description>Upper case alphabet</description></item>
		/// <item><term>0-9</term><description>Numbers</description></item>
		/// <item><term>,</term><description>Comma</description></item>
		/// <item><term>.</term><description>Period</description></item>
		/// <item><term>-</term><description>Dash</description></item>
		/// <item><term>_</term><description>Underscore</description></item>
		/// <item><term> </term><description>Other International character ranges</description></item>
		/// </list>
		/// <newpara/>
		/// Example inputs and encoded outputs:
		/// <list type="table">
		/// <item><term>alert('XSS Attack!');</term><description>alert&#40;&#39;XSS&#32;Attack&#33;&#39;&#41;&#59;</description></item>
		/// <item><term>user@contoso.com</term><description>user&#64;contoso.com</description></item>
		/// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross&#32;Site&#32;Scripting&#32;Library</description></item>
		/// </list></remarks>
		///---------------------------------------------------------------------
		public static string HtmlAttributeEncode(string input)
		{
			if (String.IsNullOrEmpty(input))
			{
				return string.Empty;
			}
			// Use a new char array.
			int len = 0;
			int tLen = input.Length;
			char[] returnMe = new char[tLen * 8]; // worst case length scenario
			char[] thisChar;
			int thisCharID;
			for (int i = 0; i < tLen; i++)
			{
				thisCharID = (int)input[i];
				if ((WhitelistCodes[thisCharID] != null)
					|| (thisCharID == 32)   //escaping space for HTMLAttribute Encoding
					)
				{
					// character needs to be encoded
					thisChar = WhitelistCodes[thisCharID];
					returnMe[len++] = '&';
					returnMe[len++] = '#';
					if (thisCharID == 32)
					{
						returnMe[len++] = '3';
						returnMe[len++] = '2';
					}
					else
					{
						for (int j = 0; j < thisChar.Length; j++)
						{
							returnMe[len++] = thisChar[j];
						}
					}
					returnMe[len++] = ';';
				}
				else
				{
					// character does not need encoding
					returnMe[len++] = input[i];
				}
			}
			return new String(returnMe, 0, len);
		}
		#endregion
		#region URLEncode_Method
		///---------------------------------------------------------------------
		/// <summary>
		/// Encodes input strings for use in universal resource locators (URLs).
		/// </summary>
		/// <param name="input">String to be encoded</param>
		/// <returns>
		///	 Encoded string for use in URLs.
		/// </returns>
		/// <remarks>
		/// This function will encode all but known safe characters.  Encoded characters are encoded using the %SINGLE_BYTE_HEX and %uDOUBLE_BYTE_HEX notation.
		/// <newpara/>
		/// Safe characters include:
		/// <list type="table">
		/// <item><term>a-z</term><description>Lower case alphabet</description></item>
		/// <item><term>A-Z</term><description>Upper case alphabet</description></item>
		/// <item><term>0-9</term><description>Numbers</description></item>
		/// <item><term>.</term><description>Period</description></item>
		/// <item><term>-</term><description>Dash</description></item>
		/// <item><term>_</term><description>Underscore</description></item>
		/// <item><term> </term><description>Other International character ranges</description></item>
		/// </list>
		/// <newpara/>
		/// Example inputs and encoded outputs:
		/// <list type="table">
		/// <item><term>alert('XSS Attack!');</term><description>alert%28%27XSS%20Attack%21%27%29%3b</description></item>
		/// <item><term>user@contoso.com</term><description>user%40contoso.com</description></item>
		/// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross%20Site%20Scripting%20Library</description></item>
		/// </list></remarks>
		///---------------------------------------------------------------------
		public static string UrlEncode(string input)
		{
			if (String.IsNullOrEmpty(input))
			{
				return string.Empty;
			}
			// Use a new char array.
			int len = 0;
			int tLen = input.Length;
			int thisCharID;
			string thisChar;
			char ch;
			Encoding inputEncoding = null;
			// Use a new char array.
			char[] returnMe = new char[tLen * 24];
			for (int i = 0; i < tLen; i++)
			{
				thisCharID = (int)input[i];
				thisChar = input[i].ToString();
				if ((WhitelistCodes[thisCharID] != null)
					|| (thisCharID == 32) || (thisCharID == 44)	//escaping SPACE and COMMA for URL Encoding
					 )
				{
					// Character needs to be encoded to default UTF-8.
					inputEncoding = Encoding.UTF8;
					byte[] inputEncodingBytes = inputEncoding.GetBytes(thisChar);
					int noinputEncodingBytes = inputEncodingBytes.Length;
					for (int index = 0; index < noinputEncodingBytes; index++)
					{
						ch = (char)inputEncodingBytes[index];
						// character needs to be encoded. Infact the byte cannot be greater than 256.
						if (ch <= 256)
						{
							returnMe[len++] = '%';
							string hex = ((int)ch).ToString("x").PadLeft(2, '0');
							returnMe[len++] = hex[0];
							returnMe[len++] = hex[1];
						}
					}
				}
				else
				{
					// character does not need encoding
					returnMe[len++] = input[i];
				}
			}
			return new String(returnMe, 0, len);
		}

		///---------------------------------------------------------------------
		/// <summary>
		/// Encodes input strings for use in universal resource locators (URLs).
		/// </summary>
		/// <param name="input">Input string</param>
		/// <param name="codepage">Codepage number of the input</param>
		/// <returns>
		///	 Encoded string for use in URLs.
		/// </returns>
		/// <remarks>
		/// This function will encodes the output as per the encoding parameter (codepage) passed to it. It will encode all but known safe characters.  Encoded characters are encoded using the %SINGLE_BYTE_HEX and %DOUBLE_BYTE_HEX notation.
		/// <newpara/>
		/// Safe characters include:
		/// <list type="table">
		/// <item><term>a-z</term><description>Lower case alphabet</description></item>
		/// <item><term>A-Z</term><description>Upper case alphabet</description></item>
		/// <item><term>0-9</term><description>Numbers</description></item>
		/// <item><term>.</term><description>Period</description></item>
		/// <item><term>-</term><description>Dash</description></item>
		/// <item><term>_</term><description>Underscore</description></item>
		/// <item><term> </term><description>Other International character ranges</description></item>
		/// </list>
		/// <newpara/>
		/// Example inputs and encoded outputs:
		/// <list type="table">
		/// <item><term>alert('XSSあAttack!');</term><description>alert%28%27XSS%82%a0Attack%21%27%29%3b</description></item>
		/// <item><term>user@contoso.com</term><description>user%40contoso.com</description></item>
		/// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross%20Site%20Scripting%20Library</description></item>
		/// </list></remarks>
		///---------------------------------------------------------------------
		public static string UrlEncode(string input, int codepage)
		{
			if (String.IsNullOrEmpty(input))
			{
				return string.Empty;
			}
			int len = 0;
			int thisCharID;
			int tLen = input.Length;
			char ch;
			string thisChar;
			Encoding inputEncoding = null;
			// Use a new char array.
			char[] returnMe = new char[tLen * 24]; // worst case length scenario
			for (int i = 0; i < tLen; i++)
			{
				thisCharID = (int)input[i];
				thisChar = input[i].ToString();
				if ((WhitelistCodes[thisCharID] != null)
					|| (thisCharID == 32) || (thisCharID == 44)	//escaping SPACE and COMMA for URL Encoding
					 )
				{
					// character needs to be encoded
					inputEncoding = Encoding.GetEncoding(codepage);
					byte[] inputEncodingBytes = inputEncoding.GetBytes(thisChar);
					int noinputEncodingBytes = inputEncodingBytes.Length;
					for (int index = 0; index < noinputEncodingBytes; index++)
					{
						ch = (char)inputEncodingBytes[index];
						// character needs to be encoded. Infact the byte cannot be greater than 256.
						if (ch <= 256)
						{
							returnMe[len++] = '%';
							string hex = ((int)ch).ToString("x").PadLeft(2, '0');
							returnMe[len++] = hex[0];
							returnMe[len++] = hex[1];
						}
					}
				}
				else
				{
					// character does not need encoding
					returnMe[len++] = input[i];
				}
			}
			return new String(returnMe, 0, len);
		}
		#endregion
		#region XMLEncode_Method
		///---------------------------------------------------------------------
		/// <summary>
		/// Encodes input strings for use in XML.
		/// </summary>
		/// <param name="input">String to be encoded</param>
		/// <returns>
		///	 Encoded string for use in XML.
		/// </returns>
		/// <remarks>
		/// This function will encode all but known safe characters.  Encoded characters are encoded using the &amp;#DECIMAL; notation.
		/// <newpara/>
		/// Safe characters include:
		/// <list type="table">
		/// <item><term>a-z</term><description>Lower case alphabet</description></item>
		/// <item><term>A-Z</term><description>Upper case alphabet</description></item>
		/// <item><term>0-9</term><description>Numbers</description></item>
		/// <item><term>,</term><description>Comma</description></item>
		/// <item><term>.</term><description>Period</description></item>
		/// <item><term>-</term><description>Dash</description></item>
		/// <item><term>_</term><description>Underscore</description></item>
		/// <item><term> </term><description>Space</description></item>
		/// <item><term> </term><description>Other International character ranges</description></item>
		/// </list>
		/// <newpara/>
		/// Example inputs and encoded outputs:
		/// <list type="table">
		/// <item><term>alert('XSS Attack!');</term><description>alert&#40;&#39;XSS Attack&#33;&#39;&#41;&#59;</description></item>
		/// <item><term>user@contoso.com</term><description>user&#64;contoso.com</description></item>
		/// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross Site Scripting Library</description></item>
		/// </list></remarks>
		///---------------------------------------------------------------------
		public static string XmlEncode(string input)
		{
			// HtmlEncode will handle input
			return HtmlEncode(input);
		}
		#endregion
		#region XMLAttributeEncode_Method
		///---------------------------------------------------------------------
		/// <summary>
		/// Encodes input strings for use in XML attributes.
		/// </summary>
		/// <param name="input">String to be encoded</param>
		/// <returns>
		///	 Encoded string for use in XML attributes.
		/// </returns>
		/// <remarks>
		/// This function will encode all but known safe characters.  Encoded characters are encoded using the &amp;#DECIMAL; notation.
		/// <newpara/>
		/// Safe characters include:
		/// <list type="table">
		/// <item><term>a-z</term><description>Lower case alphabet</description></item>
		/// <item><term>A-Z</term><description>Upper case alphabet</description></item>
		/// <item><term>0-9</term><description>Numbers</description></item>
		/// <item><term>,</term><description>Comma</description></item>
		/// <item><term>.</term><description>Period</description></item>
		/// <item><term>-</term><description>Dash</description></item>
		/// <item><term>_</term><description>Underscore</description></item>
		/// <item><term> </term><description>Other International character ranges</description></item>
		/// </list>
		/// <newpara/>
		/// Example inputs and encoded outputs:
		/// <list type="table">
		/// <item><term>alert('XSS Attack!');</term><description>alert&#40;&#39;XSS&#32;Attack&#33;&#39;&#41;&#59;</description></item>
		/// <item><term>user@contoso.com</term><description>user&#64;contoso.com</description></item>
		/// <item><term>Anti-Cross Site Scripting Library</term><description>Anti-Cross&#32;Site&#32;Scripting&#32;Library</description></item>
		/// </list></remarks>
		///---------------------------------------------------------------------
		public static string XmlAttributeEncode(string input)
		{
			//HtmlEncodeAttribute will handle input
			return HtmlAttributeEncode(input);
		}
		#endregion
		#region JavaScriptEncode_Method
		///---------------------------------------------------------------------
		/// <summary>
		/// Encodes input strings for use in JavaScript.
		/// </summary>
		/// <param name="input">String to be encoded</param>
		/// <returns>
		///	 Encoded string for use in JavaScript.
		/// </returns>
		/// <remarks>
		/// This function will encode all but known safe characters.  Encoded characters are encoded using the \xSINGLE_BYTE_HEX and \uDOUBLE_BYTE_HEX notation.
		/// <newpara/>
		/// Safe characters include:
		/// <list type="table">
		/// <item><term>a-z</term><description>Lower case alphabet</description></item>
		/// <item><term>A-Z</term><description>Upper case alphabet</description></item>
		/// <item><term>0-9</term><description>Numbers</description></item>
		/// <item><term>,</term><description>Comma</description></item>
		/// <item><term>.</term><description>Period</description></item>
		/// <item><term>-</term><description>Dash</description></item>
		/// <item><term>_</term><description>Underscore</description></item>
		/// <item><term> </term><description>Space</description></item>
		/// <item><term> </term><description>Other International character ranges</description></item>
		/// </list>
		/// <newpara/>
		/// Example inputs and encoded outputs:
		/// <list type="table">
		/// <item><term>alert('XSS Attack!');</term><description>'alert\x28\x27XSS Attack\x21\x27\x29\x3b'</description></item>
		/// <item><term>user@contoso.com</term><description>'user\x40contoso.com'</description></item>
		/// <item><term>Anti-Cross Site Scripting Library</term><description>'Anti-Cross Site Scripting Library'</description></item>
		/// </list></remarks>
		///---------------------------------------------------------------------
		public static string JavaScriptEncode(string input)
		{
			return JavaScriptEncode(input, true);
		}
		///---------------------------------------------------------------------
		/// <summary>
		/// Encodes input strings for use in JavaScript.
		/// </summary>
		/// <param name="input">String to be encoded</param>
		/// /// <param name="flagforQuote">bool flag to determin to emit quote or not. true - emit quote. false = no quote.</param>
		/// <returns>
		///	 Encoded string for use in JavaScript and does not return the output with en quotes.
		/// </returns>
		/// <remarks>
		/// This function will encode all but known safe characters.  Encoded characters are encoded using the \xSINGLE_BYTE_HEX and \uDOUBLE_BYTE_HEX notation.
		/// <newpara/>
		/// Safe characters include:
		/// <list type="table">
		/// <item><term>a-z</term><description>Lower case alphabet</description></item>
		/// <item><term>A-Z</term><description>Upper case alphabet</description></item>
		/// <item><term>0-9</term><description>Numbers</description></item>
		/// <item><term>,</term><description>Comma</description></item>
		/// <item><term>.</term><description>Period</description></item>
		/// <item><term>-</term><description>Dash</description></item>
		/// <item><term>_</term><description>Underscore</description></item>
		/// <item><term> </term><description>Space</description></item>
		/// <item><term> </term><description>Other International character ranges</description></item>
		/// </list>
		/// <newpara/>
		/// Example inputs and encoded outputs:
		/// <list type="table">
		/// <item><term>alert('XSS Attack!');</term><description>'alert\x28\x27XSS Attack\x21\x27\x29\x3b'</description></item>
		/// <item><term>user@contoso.com</term><description>'user\x40contoso.com'</description></item>
		/// <item><term>Anti-Cross Site Scripting Library</term><description>'Anti-Cross Site Scripting Library'</description></item>
		/// </list></remarks>
		///---------------------------------------------------------------------
		public static string JavaScriptEncode(string input, bool flagforQuote)
		{
			// Input validation: empty or null string condition
			if (String.IsNullOrEmpty(input))
			{
				if (flagforQuote)
				{
					return (EmptyStringJavaScript);
				}
				else
				{
					return "";
				}
			}
			// Use a new char array.
			int len = 0;
			int tLen = input.Length;
			char[] returnMe = new char[tLen * 8]; // worst case length scenario
			char[] thisChar;
			char ch;
			int thisCharID;
			// First step is to start the encoding with an apostrophe if flag is true.
			if (flagforQuote)
			{
				returnMe[len++] = '\'';
			}
			for (int i = 0; i < tLen; i++)
			{
				thisCharID = (int)input[i];
				ch = input[i];
				if (WhitelistCodes[thisCharID] != null)
				{
					// character needs to be encoded
					thisChar = WhitelistCodes[thisCharID];
					if (thisCharID > 127)
					{
						returnMe[len++] = '\\';
						returnMe[len++] = 'u';
						string hex = ((int)ch).ToString("x").PadLeft(4, '0');
						returnMe[len++] = hex[0];
						returnMe[len++] = hex[1];
						returnMe[len++] = hex[2];
						returnMe[len++] = hex[3];
					}
					else
					{
						returnMe[len++] = '\\';
						returnMe[len++] = 'x';
						string hex = ((int)ch).ToString("x").PadLeft(2, '0');
						returnMe[len++] = hex[0];
						returnMe[len++] = hex[1];
					}
				}
				else
				{
					// character does not need encoding
					returnMe[len++] = input[i];
				}
			}
			// Last step is to end the encoding with an apostrophe if flag is true.
			if (flagforQuote)
			{
				returnMe[len++] = '\'';
			}
			return new String(returnMe, 0, len);
		}
		#endregion
		#region VisualBasicScriptEncode_Method
		public static string VisualBasicScriptEncode(string input)
		{
			// Input validation: empty or null string condition
			if (String.IsNullOrEmpty(input))
			{
				return (EmptyStringVBS);
			}
			// Use a new char array.
			int len = 0;
			int tLen = input.Length;
			char[] returnMe = new char[tLen * 12]; // worst case length scenario
			char ch2;
			string temp;
			int thisCharID;
			//flag to surround double quotes around safe characters
			bool bInQuotes = false;
			for (int i = 0; i < tLen; i++)
			{
				thisCharID = (int)input[i];
				ch2 = input[i];
				if (WhitelistCodes[thisCharID] != null)
				{
					// character needs to be encoded
					// surround in quotes
					if (bInQuotes)
					{
						// get out of quotes
						returnMe[len++] = '"'; ;
						bInQuotes = false;
					}
					// adding "encoded" characters
					temp = "&chrw(" + ((uint)ch2).ToString() + ")";
					foreach (char ch in temp)
					{
						returnMe[len++] = ch;
					}
				}
				else
				{
					// character does not need encoding
					//surround in quotes
					if (!bInQuotes)
					{
						// add quotes to start
						returnMe[len++] = '&';
						returnMe[len++] = '"';
						bInQuotes = true;
					}
					returnMe[len++] = input[i];
				}
			}
			// if we're inside of quotes, close them
			if (bInQuotes)
			{
				returnMe[len++] = '"';
			}
			// finally strip extraneous "&" from beginning of the string, if necessary and RETURN
			if (returnMe.Length > 0 && returnMe[0] == '&')
			{
				return new String(returnMe, 1, len - 1);
			}
			else
			{
				return new String(returnMe, 0, len);
			}
		}
		#endregion
		#endregion
	}
}
#endregion
