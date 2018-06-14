using OnvifCamReader.OnvifServiceReference;
using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ConsoleApplication2
{
    class Program
    {
        static int Main( string[] args )
        {
            if ( args.Length < 3 )
            {
                Console.WriteLine( "Usage: OnvifCamReader.exe <url> <login> <password>" );
                return 1;
            }

            string url = args[ 0 ];
            string login = args[ 1 ];
            string password = args[ 2 ];

            var messageElement = new TextMessageEncodingBindingElement();
            messageElement.MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None);
            HttpTransportBindingElement httpBinding = new HttpTransportBindingElement();
            httpBinding.AuthenticationScheme = AuthenticationSchemes.Basic;
            CustomBinding bind = new CustomBinding(messageElement, httpBinding);

            EndpointAddress mediaAddress = new EndpointAddress( url );
            MediaClient mediaClient = new MediaClient(bind, mediaAddress);
            mediaClient.ClientCredentials.UserName.UserName = login;
            mediaClient.ClientCredentials.UserName.Password = password;

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

            return 0;
        }
    }
}
