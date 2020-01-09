using Common;
using Dapper;
using ExigoService;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ReplicatedSite.Controllers
{
    public class ImagesController : Controller
    {
        public FileStreamResult Image1(bool cache = true)
        {
            var url = string.Format("http://api.exigo.com/4.0/{0}/customerimages/{1}{2}", 
                    GlobalSettings.Exigo.Api.CompanyKey, 
                    Identity.Owner.WebAlias,
                    (!cache) ? "?s=" + Guid.NewGuid().ToString().ToLower() : string.Empty);

            return WebsiteImage(url);
        }
        public FileStreamResult Image2(bool cache = true)
        {
            var url = string.Format("http://api.exigo.com/4.0/{0}/customerimages/{1}/2{2}",
                    GlobalSettings.Exigo.Api.CompanyKey,
                    Identity.Owner.WebAlias,
                    (!cache) ? "?s=" + Guid.NewGuid().ToString().ToLower() : string.Empty);

            return WebsiteImage(url);
        }

        private FileStreamResult WebsiteImage(string url)
        {
            var defaultAvatarAsBase64 = "R0lGODlhQABAAOYAAP39/cLCwsHBwfr6+srKyvv7+8fHx/z8/Pj4+PT09MDAwPPz8+3t7cjIyM7OzsvLy/b29uDg4N3d3ebm5tHR0dPT08nJyfLy8tLS0tbW1tra2vn5+eTk5NXV1ff399nZ2erq6uHh4czMzO/v783NzdfX1+Pj48/Pz9zc3Nvb2/Hx8enp6ejo6N/f397e3uzs7PX19eXl5dTU1NDQ0PDw8Ofn57+/v+vr69jY2OLi4u7u7ry8vMPDw8bGxsTExP7+/v///8XFxQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAAAAAAALAAAAABAAEAAAAf/gD1BPYRBg4Y+hYOEgoaGio6PjY2OhZOLkYQ+h4ibkT6gjp6InZ+go6FBp6OqrK2Hq6mqoo+dPDyyp526uq2or5ueqaGskIyCgqE9m5qJvpGZy86zy5iZr9CM2ciTx9vdlZKS3tTGu9DXteqrpbCx7Yrss6LBvJ+DuYa3vPXPvLjQYvXq1I1SsniDbiUTlkhaPXAMN+GqJwtYw2ezVi1ciHGgL3uOJr6LBVGSsFrKsN0iRS3auHvowhVipm0Ss5anAggI0MOAIB47b1a65NKaNnfp8PVLFsQAAQcYMnz40OGECAI9APoDRWlrP1QUgeHj4SgADwsfQMAYUKDtAAQX/zhUMHCLLMt2/DxeI4aomg9cZyWM8AAAiOHDhgssAJHBB8++6Lp+66qIm6pGuDBMQIC4c+cFLR7oKnisdDiUnOLhyhqAwojDAAD8MPxjdu3bQA5EsPDXa0Bi/Qhm5LovgAMdtIHYrk27tuzZuUM0ELCSIjt+8y5SGu2YwIrCzZVDZ07+8AYXjj0xva4u8iJI+gxoKOC5PvPkhi9QuBvZcup5iNilyi0YMADefQiOtxx0A0zQgF0S4RMQPcHs8lcif/EQgAQH3ubhhyAylwAJAVB44YACkZNKVz4QwMJhIIono4zlbYCDAaNwFI4r8Knogwyv2VcfYtAZNgAHJKy4zf9Q2LiSilkpJCAebjMiSBt4hR3AwAwBlHgXQ6ZYJ6YPEWwwZYhoeljYDwtQoEAAvQH4EVfXVVRPCAeEV+VyMHp4GAQdiCRhNKeA018QcIYwwJCMMgpDB3D6J849F82JD5wteKBcc+OFh5uVQKiAgQJkjXZZnZzYEouGH0h5ZpppAgHADSdQJw9wJB1liSaGBHACA3puWiWMnAIxQAgETHQUX4d4c1BlyfDQwwSNVpscAjJc9l5lQ/FKC0GF6ITCopzCGmKoD0xEykbYrHtaXz8Zt4KC5uIWm7ERZDXMTGDSs65AvHSgqbWMMpAsP/Bl52QzslTDgwER5HmfsBPrmUD/CWZRg90yZEnDJLTveRJAAxPkWe4Pz31qGAQpZDVPIxUGOM0rpMFcTQ86icABAmt6Su8BC2jQgFnVxFQ0V/5WMsytt1iAwgXgNQoACBgg2rFFeNkJi1JKcfSXDTx0wIDEaBppAgE7KIBJRUGshJG7FdWkj9oURHADBCmnOYAKE2TwQAAKwCfUPTMhlSrMuBCgAQ0FPLemgpzGhkANFbSd0jQsvvdIw03+RcEKCBTpWcWdHZCABPqqxy7CADpzUlYClMB4nyd/OOVhCJxNXdvDKSXMs5ch88gtJVxQO5X1GjmBCArIo/COMwnfVwauHo885H7KykK6GH7L+U1iBkA9/8GkMzrbATXQxUxerTDVEiICzAAsebHuuSduCEhggAAYss0XU9zwgQFi0LN6GbA8P0hABTREi4PQA4CV+ICmzvWqBNWugiAg0euCVzj3+UAADlhAp4TVmfIRzDAAiECJ4pQScC3iFgSQAH3w8yn6+Qw6srGfYRhwAo85pH3tUYUAKpCAxx3QgCl7DhAKYALexew0BrlFC+hVw+zVj34xWsAD2kUTB3qCBw4A1gnHKCTDbEBo/ItWX7jiMB5oAAI0opcObXi/YQGABQ5IozKQVqm/xIBPRwykmmbjgQrwz2jBc0QDXiPIRsYICBK4XMiEYoAKLICMmCSWZzhgAQgFZ3gUPGhACjhzvxzezoI11FMqgfACCqxwfRVqRAAewAH6OBKJtsPNAjCWiYawwgcPeIERb+lIIGwABZEah8cMAULjkdBaosskkVwggM69RAEigKMq+TSje8WRm1WcERByoDalLeWDJNCm/Sy4Tjp+KoflMYENcOWDQAAAOw==";

            var imageStream = new MemoryStream();
            try
            {
                var imageUrl = url;
                var request  = (HttpWebRequest)WebRequest.Create(imageUrl);
                var response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK) throw new Exception("GOTO_CATCH");
                using (var stream = response.GetResponseStream())
                {
                    using (var tempStream = new MemoryStream())
                    {
                        stream.CopyTo(tempStream);
                        imageStream = new MemoryStream(tempStream.ToArray());
                    }
                }
            }
            catch
            {
                var bytes   = Convert.FromBase64String(defaultAvatarAsBase64);
                imageStream = new MemoryStream(bytes);
            }

            var result = new FileStreamResult(imageStream, "image/jpeg");

            result.FileDownloadName = string.Format("{0}.jpg",
                Identity.Owner.WebAlias);
            
            return result;
        }
    }
}

public enum WebsiteImageType
{
    Primary,
    Secondary
}