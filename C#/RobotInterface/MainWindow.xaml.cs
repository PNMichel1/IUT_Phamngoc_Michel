using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExtendedSerialPort_NS;
using System.Windows.Threading;
using System.Net.NetworkInformation;
using KeyboardHook_NS;
using WpfAsservissementDisplay_NS;
using SciChart.Data.Model;






namespace RobotInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {


        bool toogle, b;
        byte i;
        bool autoControlActivated;
        string receivedText;
        ExtendedSerialPort serialPort1;
        DispatcherTimer timerAffichage;
        Robot robot = new Robot();



        public MainWindow()
        {
            timerAffichage = new DispatcherTimer();
            timerAffichage.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timerAffichage.Tick += TimerAffichage_Tick;
            timerAffichage.Start();
            InitializeComponent();
            serialPort1 = new ExtendedSerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
            serialPort1.DataReceived += SerialPort1_DataReceived;
            serialPort1.Open();
            var _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyPressed += _globalKeyboardHook_KeyPressed;








        }



        public void TimerAffichage_Tick(object sender, EventArgs e)
        {

            /*  if (robot.receivedText != "")
                  //TextBoxréception.Text += receivedText;
                  TextBoxréception.Text += robot.receivedText;

                  robot.receivedText = "";
              robot.receivedText = robot.byteListReceived.ToString();
            */
            //TextBoxréception.Text= robot.receivedText;
            while (robot.byteListReceived.Count > 0)
            {
                byte Received = robot.byteListReceived.Dequeue();

                //TextBoxréception.Text += "0x" + Received.ToString("X2") + " "; //X2 c'est pour convertir en Hexadécimal 
                DecodeMessage(Received);

            }


        }

        public void SerialPort1_DataReceived(object sender, DataReceivedArgs e)
        {
            //robot.receivedText += Encoding.UTF8.GetString(e.Data, 0, e.Data.Length);

            for (int i = 0; i < e.Data.Length; i++)
            {
                robot.byteListReceived.Enqueue(e.Data[i]);


            }

        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void TextBoxEmission_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                //TextBoxréception.Text += ("Reçu : " + textBoxEmission.Text);

                serialPort1.Write(textBoxEmission.Text);

                //receivedText =textBoxEmission.Text ;

                textBoxEmission.Text = "";
            }
        }

        private void buttonEnvoyer_Click(object sender, RoutedEventArgs e)
        {

            if (!float.TryParse(Kd.Text, out float valeur))
                return;
            
            if (!float.TryParse(Ki.Text, out float valeur2))
                return;

            if (!float.TryParse(Kp.Text, out float valeur3))

                return;

            if (!float.TryParse(LineaireBox.Text, out float valeur4))

                return;
            if (!float.TryParse(AngulaireBox.Text, out float valeur5))
                return;



            List<byte> values = new List<byte>();
            values.AddRange(BitConverter.GetBytes(valeur));
            values.AddRange(BitConverter.GetBytes(valeur2));
            values.AddRange(BitConverter.GetBytes(valeur3));
            values.AddRange(BitConverter.GetBytes(valeur4));
            values.AddRange(BitConverter.GetBytes(valeur5));

            byte[] tableau = values.ToArray();


            asservSpeedDisplay.UpdatePolarSpeedCommandValues(valeur4, valeur5);
            UartEncodeAndSendMessage(0x67, tableau.Length, tableau);






            TextBoxréception.Text += ("Reçu : " + textBoxEmission.Text + "\n");
            textBoxEmission.Text = "";

            if (toogle == false)
            {
                buttonEnvoyer.Background = Brushes.RoyalBlue;
                toogle = !toogle;
            }
            else
            {
                buttonEnvoyer.Background = Brushes.Beige;
                toogle = !toogle;

            }


           


        }

        private void _globalKeyboardHook_KeyPressed(object? sender, KeyArgs e)
        {
            if (autoControlActivated == false)
            {
                switch (e.keyCode)
                {
                    case KeyCode.LEFT:
                        UartEncodeAndSendMessage(0x0051, 1, new byte[] {
                        (byte)StateRobot.STATE_TOURNE_SUR_PLACE_GAUCHE });
                        break;
                    case KeyCode.RIGHT:
                        UartEncodeAndSendMessage(0x0051, 1, new byte[] {
                    (byte)StateRobot.STATE_TOURNE_SUR_PLACE_DROITE });
                        break;
                    case KeyCode.UP:
                        UartEncodeAndSendMessage(0x0051, 1, new byte[]
                        { (byte)StateRobot.STATE_AVANCE });
                        break;
                    case KeyCode.DOWN:
                        UartEncodeAndSendMessage(0x0051, 1, new byte[]
                        { (byte)StateRobot.STATE_ARRET });
                        break;
                    case KeyCode.PAGEDOWN:
                        UartEncodeAndSendMessage(0x0051, 1, new byte[]
                        { (byte)StateRobot.STATE_RECULE });
                        break;

                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            TextBoxréception.Text = "";
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {


            /*
            byte[] bytesliste = new byte[20];
             for (byte i = 0; i < 20; i++)
             {
                 bytesliste[i] = (byte)(2 * i);

             }

             serialPort1.Write(bytesliste, 0, bytesliste.Length);
            */
            //toogle = !toogle;
            // if (toogle==true)
            //     i = 0;

            // if (toogle==false)
            //     i = 1;



            byte[] array = Encoding.ASCII.GetBytes("Decode");
            // byte[] array1 = new byte[] {i};
            //UartEncodeAndSendMessage(0x51, array1.Length, array1);
            UartEncodeAndSendMessage(0x80, array.Length, array);
            //UartEncodeAndSendMessage(0x52, array1.Length, array1);
            //UartEncodeAndSendMessage(0x20, array1.Length, array1);


            serialPort1.Write("Bonjour");





        }
        private byte CalculateChecksum(int msgFunction, int msgPayloadLength, byte[] msgPayload)
        {

            byte checksum = 0x00;


            checksum ^= 0xFE;

            checksum ^= 0x00;
            checksum ^= (byte)msgFunction;

            checksum ^= (byte)(msgPayloadLength >> 8);
            checksum ^= (byte)msgPayloadLength;

            for (int i = 0; i < msgPayloadLength; i++)
            {
                checksum ^= msgPayload[i];
                //^= c est un xor la boucle mssgpayload cree un chechsum qui sera comparé a celui calculé précedemement
            }

            return checksum;


        }



        private void TextKp_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void TextKi_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Kd_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextKd_KeyUp(object sender, KeyEventArgs e)
        {

        }

        void UartEncodeAndSendMessage(int msgFunction, int msgPayloadLength, byte[] msgPayload)
        {
            byte[] trame = new byte[msgPayloadLength + 6];
            int a = 0;
            trame[0] = 0xFE;
            trame[1] = 0x00;
            trame[2] = (byte)(msgFunction);
            trame[3] = (byte)(msgPayloadLength >> 8);
            trame[4] = (byte)(msgPayloadLength);

            for (int i = 0; i < msgPayloadLength; i++)
            {
                trame[5 + i] = (byte)(msgPayload[i]);
                a++;
            }

            trame[5 + a] = CalculateChecksum(msgFunction, msgPayloadLength, msgPayload);
            /* foreach (byte i in trame)
             {

                 TextBoxréception.Text += i;
             }
             */
            serialPort1.Write(trame, 0, trame.Length);

        }

        public enum StateRobot
        {
            STATE_ATTENTE = 0,
            STATE_ATTENTE_EN_COURS = 1,
            STATE_AVANCE = 2,
            STATE_AVANCE_EN_COURS = 3,
            STATE_TOURNE_GAUCHE = 4,
            STATE_TOURNE_GAUCHE_EN_COURS = 5,
            STATE_TOURNE_DROITE = 6,
            STATE_TOURNE_DROITE_EN_COURS = 7,
            STATE_TOURNE_SUR_PLACE_GAUCHE = 8,
            STATE_TOURNE_SUR_PLACE_GAUCHE_EN_COURS = 9,
            STATE_TOURNE_SUR_PLACE_DROITE = 10,
            STATE_TOURNE_SUR_PLACE_DROITE_EN_COURS = 11,
            STATE_ARRET = 12,
            STATE_ARRET_EN_COURS = 13,
            STATE_RECULE = 14,
            STATE_RECULE_EN_COURS = 15
        }


        void ProcessDecodedMessage(int msgFunction, int msgPayloadLength, byte[] msgPayload)
        {
            switch ((StateMessage)msgFunction)
            {
                case StateMessage.Text:
                    TextBoxréception.Text = "Text recu : " + Encoding.UTF8.GetString(msgPayload, 0, msgPayload.Length);


                    break;
                case StateMessage.Led:

                    if (msgPayload[0] == 1)
                        if (msgPayload[1] == 1)
                            Led1.IsChecked = true;
                        else
                            Led1.IsChecked = false;
                    if (msgPayload[0] == 2)
                        if (msgPayload[1] == 1)
                            Led2.IsChecked = true;
                        else
                            Led2.IsChecked = false;
                    if (msgPayload[0] == 3)
                        if (msgPayload[1] == 1)
                            Led3.IsChecked = true;
                        else
                            Led3.IsChecked = false;
                    break;

                case StateMessage.IRDistance:
                    IRG.Text = "IR Gauche : " + msgPayload[0] + " cm";
                    IRC.Text = "IR Centre : " + msgPayload[1] + " cm";
                    IRD.Text = "IR Droite : " + msgPayload[2] + " cm";
                    break;
                case StateMessage.Moteur:
                    MG.Text = "Vitesse Gauche : " + msgPayload[0] + "%";
                    MD.Text = "Vitesse Droite : " + msgPayload[1] + "%";

                    break;
                case StateMessage.Step:
                    int instant = (((int)msgPayload[1]) << 24) + (((int)msgPayload[2]) << 16)
                    + (((int)msgPayload[3]) << 8) + ((int)msgPayload[4]);
                    TextBoxréception.Text += "\nRobot␣State␣:␣" +
                    ((StateRobot)(msgPayload[0])).ToString() +
                    "␣-␣" + instant.ToString() + "␣ms";
                    break;
                case StateMessage.Encodeur:
                    robot.positionXOdo = BitConverter.ToSingle(msgPayload, 4);
                    robot.positionYOdo = BitConverter.ToSingle(msgPayload, 8);
                    robot.angleRadianFromOdometry = BitConverter.ToSingle(msgPayload, 12);
                    robot.vitesseLineaireFromOdometry = BitConverter.ToSingle(msgPayload, 16);
                    robot.vitesseAngulaireFromOdometry = BitConverter.ToSingle(msgPayload, 20);
                    X.Text = "X : " + robot.positionXOdo.ToString("N3") + " m";
                    Y.Text = "Y : " + robot.positionYOdo.ToString("N3") + " m";
                    angle.Text = "θ : " + robot.angleRadianFromOdometry.ToString("N3") + " rad";
                    asservSpeedDisplay.UpdatePolarOdometrySpeed(robot.vitesseLineaireFromOdometry, robot.vitesseAngulaireFromOdometry);        
                    
                    break;
                case StateMessage.PID_Verifiy:
                    float Kd, Ki, Kp;
                    Kp = BitConverter.ToSingle(msgPayload, 0);
                    Ki = BitConverter.ToSingle(msgPayload, 4);
                    Kd = BitConverter.ToSingle(msgPayload, 8);
                    asservSpeedDisplay.UpdatePolarSpeedCorrectionGains(Kp, 0, Ki,0, Kd, 0);
                    break;
                case StateMessage.Corr_Pid_Variables:
                
                    float erreurX = BitConverter.ToSingle(msgPayload, 0);
                    float CommandX = BitConverter.ToSingle(msgPayload, 4);
                    float KpX = BitConverter.ToSingle(msgPayload, 8);
                    float CorrPX = BitConverter.ToSingle(msgPayload, 12);
                    float erreurPMaxX = BitConverter.ToSingle(msgPayload,16);
                    float KiX = BitConverter.ToSingle(msgPayload, 20);
                    float CorrIX = BitConverter.ToSingle(msgPayload, 24);
                    float erreurIMaxX = BitConverter.ToSingle(msgPayload, 28);
                    float KdX = BitConverter.ToSingle(msgPayload, 32);
                    float CorrDX = BitConverter.ToSingle(msgPayload, 36);
                    float erreurDMaxX = BitConverter.ToSingle(msgPayload, 40);

                    float erreurT = BitConverter.ToSingle(msgPayload, 44);
                    float CommandT = BitConverter.ToSingle(msgPayload, 48);
                    float KpT = BitConverter.ToSingle(msgPayload, 52);
                    float CorrPT = BitConverter.ToSingle(msgPayload, 56);
                    float erreurPMaxT = BitConverter.ToSingle(msgPayload, 60);
                    float KiT = BitConverter.ToSingle(msgPayload, 64);
                    float CorrIT = BitConverter.ToSingle(msgPayload, 68);
                    float erreurIMaxT = BitConverter.ToSingle(msgPayload, 72);
                    float KdT = BitConverter.ToSingle(msgPayload, 76);
                    float CorrDT = BitConverter.ToSingle(msgPayload, 80);
                    float erreurDMaxT = BitConverter.ToSingle(msgPayload, 84);

                    asservSpeedDisplay.UpdatePolarSpeedErrorValues(erreurX, erreurT);
                    asservSpeedDisplay.UpdatePolarSpeedCommandValues(CommandX, CommandT);
                    asservSpeedDisplay.UpdatePolarSpeedCorrectionGains(KpX, KpT, KiX, KiT, KdX, KdT);
                    asservSpeedDisplay.UpdatePolarSpeedCorrectionValues(CorrPX, CorrPT, CorrIX, CorrIT, CorrDX, CorrDT);
                    asservSpeedDisplay.UpdatePolarSpeedCorrectionLimits(erreurPMaxX, erreurPMaxT, erreurIMaxX, erreurIMaxT, erreurDMaxX, erreurDMaxT);







                    break;
              




            }



        }


        public enum StateMessage : int
        {
            Text = 0x0080,
            Led = 0x0020,
            IRDistance = 0x0030,
            Step = 0x0050,
            PID,
            PI,
            P,
            Moteur = 0x0040,
            Encodeur = 0x0061,
            PID_Verifiy= 0x0068,
            Corr_Pid_Variables = 0x0069,





        }


        public enum StateReception
        {
            Waiting,
            FunctionMSB,
            FunctionLSB,
            PayloadLengthMSB,
            PayloadLengthLSB,
            Payload,
            CheckSum
        }
        StateReception rcvState = StateReception.Waiting;
        int msgDecodedFunction = 0;

        int msgDecodedPayloadLength = 0;
        byte[] msgDecodedPayload;
        int msgDecodedPayloadIndex = 0;

     
        

        private void DecodeMessage(byte c)
        {

            switch (rcvState)
            {
                case StateReception.Waiting:

                    if (c == 0xFE)
                        rcvState = StateReception.FunctionMSB;


                    break;
                case StateReception.FunctionMSB:
                    msgDecodedFunction = c;
                    rcvState = StateReception.FunctionLSB;

                    break;
                case StateReception.FunctionLSB:
                    msgDecodedFunction += c;

                    rcvState = StateReception.PayloadLengthMSB;
                    break;
                case StateReception.PayloadLengthMSB:
                    msgDecodedPayloadLength = (c << 8);
                    rcvState = StateReception.PayloadLengthLSB;



                    break;
                case StateReception.PayloadLengthLSB:
                    msgDecodedPayloadLength += c;
                    msgDecodedPayload = new byte[msgDecodedPayloadLength];
                    rcvState = StateReception.Payload;
                    break;
                case StateReception.Payload:

                    msgDecodedPayload[msgDecodedPayloadIndex] = c;
                    msgDecodedPayloadIndex++;
                    if (msgDecodedPayloadIndex >= msgDecodedPayloadLength)
                    {
                        rcvState = StateReception.CheckSum;
                        msgDecodedPayloadIndex = 0;

                    }




                    break;
                case StateReception.CheckSum:
                    byte calculatedChecksum = CalculateChecksum(msgDecodedFunction, msgDecodedPayloadLength, msgDecodedPayload);

                    if (calculatedChecksum == c)
                    {
                        ProcessDecodedMessage(msgDecodedFunction, msgDecodedPayloadLength, msgDecodedPayload);

                    }
                    rcvState = StateReception.Waiting;

                    break;
                default:
                    rcvState = StateReception.Waiting;
                    break;
            }
        }







    }





}
