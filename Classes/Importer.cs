﻿using PZ2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace PZ3.Classes
{
    public class Importer
    {
        public PowerGrid PowerGrid = new PowerGrid(500, 500);

        public Importer()
        {

        }

        public  void LoadModel()
        {
            LoadEntitiesFromXML();      
        }

        private void LoadEntitiesFromXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Geographic.xml");
            //xmlDoc.Load("test.xml");

            LoadSubstations(xmlDoc);
            LoadNodes(xmlDoc);
            LoadSwitches(xmlDoc);
            LoadLineEntities(xmlDoc);
        }


        private void LoadLineEntities(XmlDocument xmlDoc)
        {
            LineEntity l;

            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity");
            foreach (XmlNode node in nodeList)
            {
                l = new LineEntity();
                l.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                l.Name = node.SelectSingleNode("Name").InnerText;
                l.FirstEnd = long.Parse(node.SelectSingleNode("FirstEnd").InnerText);
                l.SecondEnd = long.Parse(node.SelectSingleNode("SecondEnd").InnerText);
                l.Resistance = double.Parse(node.SelectSingleNode("R").InnerText);

                try
                {
                    l.ConductorMaterial = (ConductorMaterial)Enum.Parse(typeof(ConductorMaterial), node.SelectSingleNode("ConductorMaterial").InnerText);
                }
                catch
                {
                    l.ConductorMaterial = ConductorMaterial.Other;
                }

                foreach (XmlNode pointNode in node.ChildNodes[9].ChildNodes) // 9 posto je Vertices 9. node u jednom line objektu
                {
                    Point p = new Point();

                    p.X = double.Parse(pointNode.SelectSingleNode("X").InnerText);
                    p.Y = double.Parse(pointNode.SelectSingleNode("Y").InnerText);

                    double x, y;
                    ToLatLon(p.X, p.Y, 34, out x, out y);

                    p.X = x;
                    p.Y = y;

                    l.Vertices.Add(p);
                }

                PowerGrid.AddConnections(l.FirstEnd);
                PowerGrid.AddConnections(l.SecondEnd);
                PowerGrid.AssignLine(l);
            }
        }

        private void LoadSwitches(XmlDocument xmlDoc)
        {
            double noviX, noviY;
            SwitchEntity switchobj;
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity");
            foreach (XmlNode node in nodeList)
            {
                switchobj = new SwitchEntity();
                switchobj.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                switchobj.Name = node.SelectSingleNode("Name").InnerText;
                switchobj.X = double.Parse(node.SelectSingleNode("X").InnerText);
                switchobj.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                switchobj.Status = node.SelectSingleNode("Status").InnerText;

                ToLatLon(switchobj.X, switchobj.Y, 34, out noviX, out noviY);
                switchobj.TranslatedX = noviX;
                switchobj.TranslatedY = noviY;
                PowerGrid.PowerEntities.Add(switchobj.Id, switchobj);
            }

        }

        private void LoadNodes(XmlDocument xmlDoc)
        {
            double noviX, noviY;
            NodeEntity nodeobj;
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity");
            foreach (XmlNode node in nodeList)
            {
                nodeobj = new NodeEntity();
                nodeobj.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                nodeobj.Name = node.SelectSingleNode("Name").InnerText;
                nodeobj.X = double.Parse(node.SelectSingleNode("X").InnerText);
                nodeobj.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                ToLatLon(nodeobj.X, nodeobj.Y, 34, out noviX, out noviY);
                nodeobj.TranslatedX = noviX;
                nodeobj.TranslatedY = noviY;
                PowerGrid.PowerEntities.Add(nodeobj.Id, nodeobj);

            }
        }

        private void LoadSubstations(XmlDocument xmlDoc)
        {
            double noviX, noviY;
            SubstationEntity sub;
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity");
            foreach (XmlNode node in nodeList)
            {
                sub = new SubstationEntity();
                sub.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                sub.Name = node.SelectSingleNode("Name").InnerText;
                sub.X = double.Parse(node.SelectSingleNode("X").InnerText);
                sub.Y = double.Parse(node.SelectSingleNode("Y").InnerText);

                ToLatLon(sub.X, sub.Y, 34, out noviX, out noviY);
                sub.TranslatedX = noviX;
                sub.TranslatedY = noviY;

                PowerGrid.PowerEntities.Add(sub.Id, sub);
            }
        }

        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }

    }
}
