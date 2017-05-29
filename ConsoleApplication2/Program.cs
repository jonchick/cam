using ConsoleApplication2.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            var messageElement = new TextMessageEncodingBindingElement();
            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
            httpBinding.AuthenticationScheme = AuthenticationSchemes.Basic;
            CustomBinding bind = new CustomBinding(messageElement, httpBinding);
            EndpointAddress mediaAddress = new EndpointAddress("http://10.0.0.42:8899/onvif/deviceservice");
            MediaClient mediaClient = new MediaClient(bind, mediaAddress);
            mediaClient.ClientCredentials.UserName.UserName = "admin";
            mediaClient.ClientCredentials.UserName.Password = "xp15admin";
            Profile[] profiles = mediaClient.GetProfiles();
            string profileToken = profiles[0].token;
            MediaUri mediaUri = mediaClient.GetSnapshotUri(profileToken);

            StreamSetup streamSetup = new StreamSetup();
            streamSetup.Stream = StreamType.RTPUnicast;

            streamSetup.Transport = new Transport();
            streamSetup.Transport.Protocol = TransportProtocol.RTSP;

            MediaUri mediaStreamUri = mediaClient.GetStreamUri(streamSetup, profileToken);

            Console.WriteLine( mediaUri.Uri.ToString() );
            Console.WriteLine( mediaStreamUri.Uri.ToString() );
        }
    }
}
