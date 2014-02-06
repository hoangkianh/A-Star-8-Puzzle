/*
 * Created by SharpDevelop.
 * User: HKA_250
 * Date: 26/08/2012
 * Time: 12:39 | HKA
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace A_Star_8_Puzzle
{
	/// <summary>
	/// Class Status_Node dùng để lưu trạng thái hiện tại của ma trận Puzzle
	/// Mỗi node có các thuộc tính như phía dưới
	/// </summary>
	public class Status_Node
	{
		private Status_Node m_Parent;	// node cha
		
		/* chuỗi của node [theo quy tắc từ trên xuống dưới, từ trái -> phải]
			VD: 203184765
			Biểu diễn thành: 2 0 3
							 1 8 4
							 7 6 5
		 */
		private string m_Code;
		
		/* 
		 * Vị trí chính xác của 1 ô số trên ma trận 8 Puzzle
		 * Xác định vị trí hàng cột của ô số. Theo cách tính: m_Map[x, y] = m_Map[k / 3, k % 3] với k là giá trị ô số
		 * VD: Ô số 7 thì k = 6 (tính từ 0)
		 * => Có vị trí chính xác là m_Map[6/3, 6%3] = [2, 0] -> hàng 2, cột 0
		 * */
		private char[,] m_Map = new char[3, 3];
		private int m_G; 	// giá trị của hàm g tại 1 Node (trong bài này lấy theo bậc của node. Node đầu tiên = 0 và tăng dần)
		private int m_H;	// giá trị của hàm h tại 1 Node (là số các ô sai vị trí)
		private Font m_Font;	// font 
		
		public string Code
		{
			get {return m_Code;}
			set
			{
				m_Code = value;
				for(int i = 0; i < 9; i++)
				{
					m_Map[i / 3,  i % 3] = value[i]; // tính vị trí chính xác của ô số
				}
			}
		}
		
		public Status_Node Parent
		{
			get {return m_Parent;}
			set {m_Parent = value;}
		}
		
		public char[,] Map
		{
			get {return m_Map;}
			set 
			{
				m_Map = value;
				StringBuilder sb = new StringBuilder(9);
				
				for(int i = 0; i < 3; i++)
				{
					for(int j = 0; j < 3; j++)
					{
						sb.Append(value[i, j]);
					}
				}
				m_Code = sb.ToString();
			}
		}
		
		// hàm đánh giá
		public int F
		{
			get {return m_G + m_H;}
		}

        public int G
        {
            get {return m_G;}
            set {m_G = value;}
        }

        public int H
        {
            get {return m_H;}
            set {m_H = value;}
        }
		
		public Status_Node(string code, Status_Node parent, int g, int h)
		{
			Code = code;
            m_Parent = parent;
            m_G = g;
            m_H = h;
            m_Font = new Font("Candara", 25F, FontStyle.Regular, GraphicsUnit.Point);
		}

        // vẽ trạng thái
        public void Paint (Graphics g) 
        {
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(m_Map[i, j] != '0')
                    {
                        g.FillRectangle(Brushes.DarkCyan, 96*j, 96*i, 95, 95);
                        g.DrawString(m_Map[i, j].ToString(), m_Font, Brushes.White, 95*j + 26, 95*i + 26);
                    }
                }
            }
        }
	}
}
