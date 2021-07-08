using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace AndroidVectorViewer {
    class VectorDrawable {

        private string mPath;
        private List<VectorCommand> mCommandsImpl = new List<VectorCommand>();

        private char[] map = { 'L', 'l', 'H', 'h', 'V', 'v', 'Z', 'z', 'C', 'c', 'S', 's', 'Q', 'q', 'T', 't', 'A', 'a', 'M', 'm' };
        public VectorDrawable(string path ) {
            mPath = path;
        }
        /*
        android:pathData=
        "M12,2C8.13,2 5,5.13 5,9c0,5.25 7,13 7,13s7,-7.75 7,-13c0,-3.87 -3.13,-7 -7,-7z
        M12,11.5c-1.38,0 -2.5,-1.12 -2.5,-2.5s1.12,-2.5 2.5,-2.5 2.5,1.12 2.5,2.5 -1.12,2.5 -2.5,2.5z" 

            https://www.w3.org/TR/SVG/paths.html
        */
        public void draw(Panel p) {

            IList<string> tmp = SplitAndKeepDelimiters( mPath, map );
            int i = 0;
            for(int k = 0; k < tmp.Count; k+=2 ) {
                Console.WriteLine( tmp[k] );
                if ( k < tmp.Count  ) {
                    VectorCommand c = new VectorCommand();
                    char curCommand = tmp[k][0];
                    if ( curCommand == 'z' || curCommand == 'Z' ) {
                        c.setCommand( curCommand );
                        c.setPath( "" );
                        k--;
                    } else {
                        c.setCommand( curCommand );
                        c.setPath( tmp[k + 1] );
                    }
                    mCommandsImpl.Add( c );
                }
            }

            // now start drawing
            // it wont work since its called via the main constructor.
            // so it builds,runs, paints , and the constructor calls onPaint() again.
            // if you drag n drop an xml, it works
            Graphics g = p.CreateGraphics();
            Pen pen = new Pen( Color.Red,10 );
            SolidBrush b = new SolidBrush( Color.Red );
            foreach ( VectorCommand s in mCommandsImpl ) {
                if (s.getCommand() == 'C') {
                    
                    string[] splitted = s.getPath().ToString().Split( ' ' );
                    Point[] points = new Point[splitted.Length];
                    for (int k=0;k<splitted.Length;k++ ) {
                        points[k] = new Point( 
                            (int) Convert.ToDouble( splitted[k].Split( ',' )[0] ) * 24,
                              (int)Convert.ToDouble( splitted[k].Split( ',' )[1] ) * 24 );
                        Console.WriteLine(points[k].ToString() + "<----" );
                    }
                    g.DrawCurve( pen, points );
                    break;
                   
                }
            }
            

        }

        public void printToLog(TextBox tb ) {
            tb.Text += "printing fixed commands \r\n";
            foreach (VectorCommand s in mCommandsImpl )
                tb.Text += s.getCommand() + s.getPath() + "\r\n";
        }

        public static IList<string> SplitAndKeepDelimiters( string s, params char[] delimiters ) {
            var parts = new List<string>();
            if ( !string.IsNullOrEmpty( s ) ) {
                int iFirst = 0;
                do {
                    int iLast = s.IndexOfAny( delimiters, iFirst );
                    if ( iLast >= 0 ) {
                        if ( iLast > iFirst )
                            parts.Add( s.Substring( iFirst, iLast - iFirst ) ); //part before the delimiter
                        parts.Add( new string( s[iLast], 1 ) );//the delimiter
                        iFirst = iLast + 1;
                        continue;
                    }

                    //No delimiters were found, but at least one character remains. Add the rest and stop.
                    parts.Add( s.Substring( iFirst, s.Length - iFirst ) );
                    break;

                } while ( iFirst < s.Length );
            }

            return parts;
        }
    }
 
}
