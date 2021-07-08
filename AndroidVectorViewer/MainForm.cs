using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AndroidVectorViewer {
    public partial class MainForm : Form {

        private static string NAMESPACE = "android:";

        private static string DEBUG_FILE = "ic_about_perms_location.xml";
        private string mFileDir;
        private List<string> mDrawPaths = new List<string>();
        public MainForm() {
            InitializeComponent();
        }

        private void Form1_Load( object sender, EventArgs e ) {
            mFileDir = DEBUG_FILE;

           
            readXML();
            panel_main.Invalidate();
        }

        private void panel1_DragDrop( object sender, DragEventArgs e ) {
            string[] files = (string[])e.Data.GetData( DataFormats.FileDrop );
            string myfile = files[0] ;
            foreach ( string file in files ) {
                log( file );
            }
            mFileDir = myfile;

            readXML();
        }

        private void panel_main_DragEnter( object sender, DragEventArgs e ) {
            if ( e.Data.GetDataPresent( DataFormats.FileDrop ) ) e.Effect = DragDropEffects.Copy;
        }

        private void log(string line ) {
            tb_debug.Text += line + "\r\n";
        }

        private void readXML() {
            // parse the xml properties
            XmlTextReader textReader = new XmlTextReader( mFileDir );
            while ( textReader.Read() ) {
                // Move to fist element  
                textReader.MoveToElement();
                if ( (textReader.NodeType == XmlNodeType.Element) && (textReader.Name == "vector") ) {
                    if ( textReader.HasAttributes ) {
                        log( "width: " + textReader.GetAttribute( NAMESPACE + "width" ) );
                        log( "height: " + textReader.GetAttribute( NAMESPACE + "height" ) );
                        log( "viewportWidth: " + textReader.GetAttribute( NAMESPACE + "viewportWidth" ) );
                        log( "viewportHeight: " + textReader.GetAttribute( NAMESPACE + "viewportHeight" ) );
                    }
                }
                if ( (textReader.NodeType == XmlNodeType.Element) && (textReader.Name == "path") ) {
                    if ( textReader.HasAttributes ) {
                        string tmpPath = textReader.GetAttribute( NAMESPACE + "pathData" );
                        mDrawPaths.Add(tmpPath);
                        log( "path: " + tmpPath );
                        log( "path length (max 800): " + tmpPath.Length );
                    }
                }
            }
            // since file parse is done, start drawing
            drawPath();
        }
        
        private void drawPath() {
            
            // this foreach supports an xml with many <vector> paths
            foreach (String s in mDrawPaths ) {
                VectorDrawable vectorDrawable = new VectorDrawable( s );
                vectorDrawable.draw( panel_main );
                vectorDrawable.printToLog( tb_debug );
            }
            
        }

        private void panel_main_MouseMove( object sender, MouseEventArgs e ) {
            lb_mouse.Text = e.X + "," + e.Y;
        }
 
    }
}
