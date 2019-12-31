using codessentials.CGM.Classes;
using codessentials.CGM.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.Drawing;

namespace codessentials.CGM.Tests
{
    [TestFixture]
    public class CommandTests
    {
        private static CGMColor Color_Index = new CGMColor() { ColorIndex = 2 };
        private static CGMColor Color_Index2 = new CGMColor() { ColorIndex = 3 };
        private static CGMColor Color_Color = new CGMColor() { Color = System.Drawing.Color.Red };
        private static CGMColor Color_Color2 = new CGMColor() { Color = System.Drawing.Color.Peru };
        private static CGMPoint Point = new CGMPoint(2, 2);
        private static CGMPoint Point2 = new CGMPoint(5, 8);
        private static CGMPoint Point3 = new CGMPoint(4, 99);

        [Test]
        public void AlternateCharacterSetIndex_Write_Binary()
        {
            TestCommand(cgm => new AlternateCharacterSetIndex(cgm, 2), cmd => cmd.Index == 2);
        }

        [Test]
        public void AppendText_Write_Binary()
        {
            TestCommand(cgm => new AppendText(cgm, AppendText.FinalEnum.FINAL, "test"), cmd => cmd.Final == AppendText.FinalEnum.FINAL && cmd.Text == "test");
            TestCommand(cgm => new AppendText(cgm, AppendText.FinalEnum.NOTFINAL, "test2"), cmd => cmd.Final == AppendText.FinalEnum.NOTFINAL && cmd.Text == "test2");
        }

        [Test]
        public void ApplicationStructureAttribute_Write_Binary()
        {
            var sdr = new StructuredDataRecord();
            sdr.Add(StructuredDataRecord.StructuredDataType.S, new object[] { "lala" });

            TestCommand(cgm => new ApplicationStructureAttribute(cgm, "test", sdr), cmd =>
            {
                cmd.AttributeType.Should().Be("test");
                cmd.Data.Members.Should().HaveCount(1);
                cmd.Data.Members[0].Type.Should().Be(StructuredDataRecord.StructuredDataType.S);
                cmd.Data.Members[0].Data[0].Should().Be("lala");
            });
        }

        [Test]
        public void ApplicationStructureDirectory_Write_Binary()
        {
            var info = new ApplicationStructureDirectory.ApplicationStructureInfo() { Identifier = "A", Location = 54 };
            var info2 = new ApplicationStructureDirectory.ApplicationStructureInfo() { Identifier = "AAAAAAAAAABBBBBBBBBBCCCCCCCCCC".PadRight(400, 'G'), Location = 2 };
            var info3 = new ApplicationStructureDirectory.ApplicationStructureInfo() { Identifier = "Aasdasdasdasdasdasd3 sd fsf 324 äö", Location = 42 };

            TestCommand(cgm => new ApplicationStructureDirectory(cgm, ApplicationStructureDirectory.DataTypeSelector.UI16, new[] { info }),
                cmd => cmd.TypeSelector == ApplicationStructureDirectory.DataTypeSelector.UI16 && cmd.Infos[0].Identifier == "A" && cmd.Infos[0].Location == 54);

            TestCommand(cgm => new ApplicationStructureDirectory(cgm, ApplicationStructureDirectory.DataTypeSelector.UI32, new[] { info2, info3 }),
                cmd => cmd.TypeSelector == ApplicationStructureDirectory.DataTypeSelector.UI32 && cmd.Infos[0].Identifier == info2.Identifier && cmd.Infos[1].Location == info3.Location);
        }

        [Test]
        public void AspectSourceFlags_Write_Binary()
        {
            var info = new AspectSourceFlags.ASFInfo() { Type = AspectSourceFlags.ASFType.edgecolour, Value = AspectSourceFlags.ASFValue.BUNDLED };
            var info2 = new AspectSourceFlags.ASFInfo() { Type = AspectSourceFlags.ASFType.hatchindex, Value = AspectSourceFlags.ASFValue.INDIV };
            var info3 = new AspectSourceFlags.ASFInfo() { Type = AspectSourceFlags.ASFType.textcolour, Value = AspectSourceFlags.ASFValue.BUNDLED };

            TestCommand(cgm => new AspectSourceFlags(cgm, new[] { info }),
                cmd => cmd.Infos[0].Type == info.Type && cmd.Infos[0].Value == info.Value);

            TestCommand(cgm => new AspectSourceFlags(cgm, new[] { info2, info3 }),
                cmd => cmd.Infos[0].Type == info2.Type && cmd.Infos[0].Value == info2.Value && cmd.Infos[1].Type == info3.Type);
        }

        [Test]
        public void AuxiliaryColour_Write_Binary_ColorMode_Indexed()
        {
            TestCommand(cgm => new AuxiliaryColour(cgm, Color_Index), cmd => IsColorIndex(cmd.Color));
            TestCommand(cgm => new AuxiliaryColour(cgm, Color_Index2), cmd => IsColorIndex2(cmd.Color));
        }

        [Test]
        public void AuxiliaryColour_Write_Binary_ColorMode_Direct()
        {
            TestCommand(cgm =>
            {
                cgm.Commands.Add(new ColourSelectionMode(cgm, ColourSelectionMode.Type.DIRECT));
                cgm.ColourSelectionMode = ColourSelectionMode.Type.DIRECT;
                return new AuxiliaryColour(cgm, Color_Color);
            }, cmd => cmd.Color.Color.ToArgb().Should().Be(Color_Color.Color.ToArgb()));

            TestCommand(cgm =>
            {
                cgm.Commands.Add(new ColourSelectionMode(cgm, ColourSelectionMode.Type.DIRECT));
                cgm.ColourSelectionMode = ColourSelectionMode.Type.DIRECT;
                return new AuxiliaryColour(cgm, Color_Color2);
            }, cmd => cmd.Color.Color.ToArgb().Should().Be(Color_Color2.Color.ToArgb()));
        }

        [Test]
        public void BackgroundColour_Write_Binary()
        {
            TestCommand(cgm => new BackgroundColour(cgm, System.Drawing.Color.Red), cmd => cmd.Color.ToArgb().Should().Be(System.Drawing.Color.Red.ToArgb()));
            TestCommand(cgm => new BackgroundColour(cgm, System.Drawing.Color.Purple), cmd => cmd.Color.ToArgb().Should().Be(System.Drawing.Color.Purple.ToArgb()));
        }

        [Test]
        public void BeginApplicationStructure_Write_Binary()
        {
            TestCommand(cgm => new BeginApplicationStructure(cgm, "aa", "cc", BeginApplicationStructure.InheritanceFlag.APS), cmd => cmd.Id == "aa" && cmd.Type == "cc" && cmd.Flag == BeginApplicationStructure.InheritanceFlag.APS);
            TestCommand(cgm => new BeginApplicationStructure(cgm, "cccccccccccsdfsd sf", "as454 fdgdfgdfg", BeginApplicationStructure.InheritanceFlag.STLIST), cmd => cmd.Id == "cccccccccccsdfsd sf" && cmd.Type == "as454 fdgdfgdfg" && cmd.Flag == BeginApplicationStructure.InheritanceFlag.STLIST);
        }

        [Test]
        public void BeginApplicationStructureBody_Write_Binary()
        {
            TestCommand(cgm => new BeginApplicationStructureBody(cgm), cmd => true);
        }

        [Test]
        public void BeginCompoundLine_Write_Binary()
        {
            TestCommand(cgm => new BeginCompoundLine(cgm), cmd => true);
        }

        [Test]
        public void BeginCompoundTextPath_Write_Binary()
        {
            TestCommand(cgm => new BeginCompoundTextPath(cgm), cmd => true);
        }

        [Test]
        public void BeginFigure_Write_Binary()
        {
            TestCommand(cgm => new BeginFigure(cgm), cmd => true);
        }

        [Test]
        public void BeginMetafile_Write_Binary()
        {
            TestCommand(cgm => new BeginMetafile(cgm, "test"), cmd => cmd.FileName == "test");
            TestCommand(cgm => new BeginMetafile(cgm, "tes".PadRight(300)), cmd => cmd.FileName == "tes".PadRight(300));
            TestCommand(cgm => new BeginMetafile(cgm, ""), cmd => cmd.FileName == "");
        }

        [Test]
        public void BeginPicture_Write_Binary()
        {
            TestCommand(cgm => new BeginPicture(cgm, "test"), cmd => cmd.PictureName == "test");
            TestCommand(cgm => new BeginPicture(cgm, "tes".PadRight(300)), cmd => cmd.PictureName == "tes".PadRight(300));
            TestCommand(cgm => new BeginPicture(cgm, ""), cmd => cmd.PictureName == "");
        }

        [Test]
        public void BeginPictureBody_Write_Binary()
        {
            TestCommand(cgm => new BeginPictureBody(cgm), cmd => true);
        }

        [Test]
        public void BeginProtectionRegion_Write_Binary()
        {
            TestCommand(cgm => new BeginProtectionRegion(cgm, 1), cmd => cmd.RegionIndex == 1);
            TestCommand(cgm => new BeginProtectionRegion(cgm, 0), cmd => cmd.RegionIndex == 0);
        }

        [Test]
        public void BeginSegment_Write_Binary()
        {
            TestCommand(cgm => new BeginSegment(cgm, 1), cmd => cmd.Id == 1);
            TestCommand(cgm => new BeginSegment(cgm, 0), cmd => cmd.Id == 0);
            TestCommand(cgm => new BeginSegment(cgm, 13), cmd => cmd.Id == 13);
        }

        [Test]
        public void BeginTileArray_Write_Binary_ColorMode_Direct()
        {
            var position = new CGMPoint(55.879, 1.654889);
            var cellPathDirection = 2;
            var lineProgressionDirection = 5;
            var nTilesInPathDirection = 650;
            var nTilesInLineDirection = 87;
            var nCellsPerTileInPathDirection = 5;
            var nCellsPerTileInLineDirection = 3;
            var cellSizeInPathDirection = 10.2;
            double cellSizeInLineDirection = 5;
            var imageOffsetInPathDirection = 0;
            var imageOffsetInLineDirection = 1;
            var nCellsInPathDirection = 5;
            var nCellsInLineDirection = 5;

            Func<CGMFile, BeginTileArray> tileArrayFunc = cgm =>
            {
                return new BeginTileArray(cgm, position, cellPathDirection, lineProgressionDirection, nTilesInPathDirection, nTilesInLineDirection,
                    nCellsPerTileInPathDirection, nCellsPerTileInLineDirection, cellSizeInPathDirection, cellSizeInLineDirection, imageOffsetInPathDirection,
                    imageOffsetInLineDirection, nCellsInPathDirection, nCellsInLineDirection);
            };

            Func<BeginTileArray, bool> checkFunc = cmd =>
            {
                return cmd.Position.X == position.X && cmd.Position.Y == position.Y
                && cmd.CellPathDirection == cellPathDirection
                && cmd.LineProgressionDirection == lineProgressionDirection
                && cmd.NumberTilesInPathDirection == nTilesInPathDirection
                && cmd.NumberTilesInLineDirection == nTilesInLineDirection
                && cmd.NumberCellsInPathDirection == nCellsInPathDirection
                && cmd.NumberCellsInLineDirection == nCellsInLineDirection
                && cmd.NumberCellsPerTileInPathDirection == nCellsPerTileInPathDirection
                && cmd.NumberCellsPerTileInLineDirection == nCellsPerTileInLineDirection
                && cmd.CellSizeInPathDirection == cellSizeInPathDirection
                && cmd.CellSizeInLineDirection == cellSizeInLineDirection
                && cmd.ImageOffsetInPathDirection == imageOffsetInPathDirection
                && cmd.ImageOffsetInLineDirection == imageOffsetInLineDirection;
            };

            TestCommand(cgm =>
            {
                cgm.Commands.Add(new VDCType(cgm, VDCType.Type.Real));
                cgm.Commands.Add(new VDCRealPrecision(cgm, Precision.Floating_32));
                cgm.Commands.Add(new RealPrecision(cgm, Precision.Floating_32));
                cgm.VDCType = VDCType.Type.Real;
                cgm.VDCRealPrecision = Precision.Floating_32;
                cgm.RealPrecision = Precision.Floating_32;

                return tileArrayFunc(cgm);
            }, checkFunc);
        }

        [Test]
        public void BitonalTile_Write_Binary()
        {
            var color1 = new CGMColor() { ColorIndex = 5 };
            var color2 = new CGMColor() { ColorIndex = 4 };

            var sdr = new StructuredDataRecord();
            sdr.Add(StructuredDataRecord.StructuredDataType.E, new object[] { 2 });
            var image = new MemoryStream(new byte[] { 1, 20, 30, 5, 45 });

            TestCommand(cgm => new BitonalTile(cgm, CompressionType.BITMAP, 1, color1, color2, sdr, image), cmd => {
                cmd.CompressionType.Should().Be(CompressionType.BITMAP);
                cmd.RowPaddingIndicator.Should().Be(1);
                cmd.Backgroundcolor.Should().Be(color1);
                cmd.Foregroundcolor.Should().Be(color2);
            });

            TestCommand(cgm => new BitonalTile(cgm, CompressionType.PNG, 88, color1, color2, sdr, image), cmd =>
            {
                cmd.CompressionType.Should().Be(CompressionType.PNG);
                cmd.RowPaddingIndicator.Should().Be(88);
                cmd.Backgroundcolor.Should().Be(color1);
                cmd.Foregroundcolor.Should().Be(color2);
                cmd.DataRecord.Members.Should().HaveCount(1);
                cmd.DataRecord.Members[0].Type.Should().Be(StructuredDataRecord.StructuredDataType.E);
                cmd.Image.ToArray().Should().ContainInOrder( image.ToArray());
            });
        }

        [Test]
        public void CellArray_Write_Binary()
        {
            var point1 = new CGMPoint(1, 1);
            var point2 = new CGMPoint(2, 2);

            TestCommand(cgm => new CellArray(cgm, 0, 1, 2, point1, point2, point2, 0, new[] { Color_Index, Color_Index2 }), cmd =>
            {
                cmd.RepresentationFlag.Should().Be(0);
                cmd.Nx.Should().Be(1);
                cmd.Ny.Should().Be(2);
                cmd.P.Should().Be(point1);
                cmd.Q.Should().Be(point2);
                cmd.R.Should().Be(point2);
                cmd.LocalColorPrecision.Should().Be(0);
                cmd.Colors[0].Should().Be(Color_Index);
                cmd.Colors[1].Should().Be(Color_Index2);
            });

            TestCommand(cgm => new CellArray(cgm, 1, 1, 2, point1, point2, point2, 8, new[] { Color_Index, Color_Index2 }), cmd =>
            {
                cmd.RepresentationFlag.Should().Be(1);
                cmd.Nx.Should().Be(1);
                cmd.Ny.Should().Be(2);
                cmd.P.Should().Be(point1);
                cmd.Q.Should().Be(point2);
                cmd.R.Should().Be(point2);
                cmd.LocalColorPrecision.Should().Be(8);
                cmd.Colors[0].Should().Be(Color_Index);
                cmd.Colors[1].Should().Be(Color_Index2);
            });
        }

        [Test]
        public void CharacterCodingAnnouncer_Write_Binary()
        {
            TestCommand(cgm => new CharacterCodingAnnouncer(cgm, CharacterCodingAnnouncer.Type.BASIC_7_BIT), cmd => cmd.Value.Should().Be(CharacterCodingAnnouncer.Type.BASIC_7_BIT));
            TestCommand(cgm => new CharacterCodingAnnouncer(cgm, CharacterCodingAnnouncer.Type.BASIC_8_BIT), cmd => cmd.Value.Should().Be(CharacterCodingAnnouncer.Type.BASIC_8_BIT));
            TestCommand(cgm => new CharacterCodingAnnouncer(cgm, CharacterCodingAnnouncer.Type.EXTENDED_8_BIT), cmd => cmd.Value.Should().Be(CharacterCodingAnnouncer.Type.EXTENDED_8_BIT));
        }

        [Test]
        public void CharacterExpansionFactore_Write_Binary()
        {
            TestCommand(cgm => new CharacterExpansionFactor(cgm, 12.2), cmd => cmd.Factor.Should().Be(12.199996948242188));
            TestCommand(cgm => new CharacterExpansionFactor(cgm, 5), cmd => cmd.Factor.Should().Be(5));
            TestCommand(cgm => new CharacterExpansionFactor(cgm, 45.689), cmd => cmd.Factor.Should().Be(45.688995361328125));
        }

        [Test]
        public void CharacterHeight_Write_Binary()
        {
            TestCommand(cgm =>
            {
                cgm.Commands.Add(new VDCRealPrecision(cgm, Precision.Fixed_32));
                cgm.VDCRealPrecision = Precision.Fixed_32;
                cgm.Commands.Add(new VDCType(cgm, VDCType.Type.Real));
                cgm.VDCType = VDCType.Type.Real;
                return new CharacterHeight(cgm, 12.2);
            }, cmd => cmd.Height.Should().Be(12.199996948242188));

            TestCommand(cgm => new CharacterHeight(cgm, 5), cmd => cmd.Height.Should().Be(5));
        }

        [Test]
        public void CharacterOrientationt_Write_Binary()
        {
            TestCommand(cgm =>
            {
                cgm.Commands.Add(new VDCRealPrecision(cgm, Precision.Fixed_32));
                cgm.VDCRealPrecision = Precision.Fixed_32;
                cgm.Commands.Add(new VDCType(cgm, VDCType.Type.Real));
                cgm.VDCType = VDCType.Type.Real;
                return new CharacterOrientation(cgm, 12.2, 1, 5.5, 4);
            }, cmd =>
            {
                cmd.Xup.Should().Be(12.199996948242188);
                cmd.yup.Should().Be(1);
                cmd.Xbase.Should().Be(5.5);
                cmd.Ybase.Should().Be(4);
            });

            TestCommand(cgm => new CharacterOrientation(cgm, 5, 3, 2, 1), cmd =>
            {
                cmd.Xup.Should().Be(5);
                cmd.yup.Should().Be(3);
                cmd.Xbase.Should().Be(2);
                cmd.Ybase.Should().Be(1);
            });
        }

        [Test]
        public void CharacterSetList_Write_Binary()
        {
            var item = new KeyValuePair<CharacterSetList.Type, string>(CharacterSetList.Type._94_CHAR_G_SET, "B");
            var item2 = new KeyValuePair<CharacterSetList.Type, string>(CharacterSetList.Type._96_CHAR_G_SET, "A");
            var item3 = new KeyValuePair<CharacterSetList.Type, string>(CharacterSetList.Type.COMPLETE_CODE, "I");
            var item4 = new KeyValuePair<CharacterSetList.Type, string>(CharacterSetList.Type.COMPLETE_CODE, "L");

            TestCommand(cgm => new CharacterSetList(cgm, new[] { item }), cmd => cmd.CharacterSets[0].Key == item.Key && cmd.CharacterSets[0].Value == item.Value);
            TestCommand(cgm => new CharacterSetList(cgm, new[] { item2, item3, item4 }), cmd => cmd.CharacterSets[0].Key == item2.Key && cmd.CharacterSets[0].Value == item2.Value && cmd.CharacterSets[1].Key == item3.Key && cmd.CharacterSets[1].Value == item3.Value);
        }

        [Test]
        public void CharacterSetIndex_Write_Binary()
        {
            TestCommand(cgm => new CharacterSetIndex(cgm, 1), cmd => cmd.Index.Should().Be(1));
            TestCommand(cgm => new CharacterSetIndex(cgm, 0), cmd => cmd.Index.Should().Be(0));
        }

        [Test]
        public void CharacterSpacing_Write_Binary()
        {
            TestCommand(cgm => new CharacterSpacing(cgm, 2), cmd => cmd.Space.Should().Be(2));
        }

        [Test]
        public void CircleElement_Write_Binary()
        {
            var point = new CGMPoint(2, 2);

            TestCommand(cgm => new CircleElement(cgm, point, 2), cmd =>
            {
                cmd.Center.Should().Be(point);
                cmd.Radius.Should().Be(2);
            });
        }

        [Test]
        public void CircularArc3Point_Write_Binary()
        {
            var point = new CGMPoint(2, 2);
            var point2 = new CGMPoint(5, 2);
            var point3 = new CGMPoint(4, 4);

            TestCommand(cgm => new CircularArc3Point(cgm, point, point2, point3), cmd =>
            {
                cmd.P1.Should().Be(point);
                cmd.P2.Should().Be(point2);
                cmd.P3.Should().Be(point3);
            });
        }

        [Test]
        public void CircularArc3PointClose_Write_Binary()
        {
            var point = new CGMPoint(2, 2);
            var point2 = new CGMPoint(5, 2);
            var point3 = new CGMPoint(4, 4);

            TestCommand(cgm => new CircularArc3PointClose(cgm, point, point2, point3, ClosureType.CHORD), cmd =>
            {
                cmd.P1.Should().Be(point);
                cmd.P2.Should().Be(point2);
                cmd.P3.Should().Be(point3);
                cmd.Type.Should().Be(ClosureType.CHORD);
            });
        }

        [Test]
        public void CircularArcCentre_Write_Binary()
        {
            var point = new CGMPoint(2, 2);

            TestCommand(cgm => new CircularArcCentre(cgm, point, 1, 2, 3, 4, 5), cmd =>
            {
                cmd.Center.Should().Be(point);
                cmd.StartDeltaX.Should().Be(1);
                cmd.StartDeltaY.Should().Be(2);
                cmd.EndDeltaX.Should().Be(3);
                cmd.EndDeltaY.Should().Be(4);
                cmd.Radius.Should().Be(5);
            });
        }

        [Test]
        public void CircularArcCentreClose_Write_Binary()
        {
            var point = new CGMPoint(2, 2);

            TestCommand(cgm => new CircularArcCentreClose(cgm, point, 1, 2, 3, 4, 5, ClosureType.PIE), cmd =>
            {
                cmd.Center.Should().Be(point);
                cmd.StartDeltaX.Should().Be(1);
                cmd.StartDeltaY.Should().Be(2);
                cmd.EndDeltaX.Should().Be(3);
                cmd.EndDeltaY.Should().Be(4);
                cmd.Radius.Should().Be(5);
                cmd.Type.Should().Be(ClosureType.PIE);
            });
        }

        [Test]
        public void CircularArcCentreReversed_Write_Binary()
        {
            var point = new CGMPoint(2, 2);

            TestCommand(cgm => new CircularArcCentreReversed(cgm, point, 1, 2, 3, 4, 5), cmd =>
            {
                cmd.Center.Should().Be(point);
                cmd.StartDeltaX.Should().Be(1);
                cmd.StartDeltaY.Should().Be(2);
                cmd.EndDeltaX.Should().Be(3);
                cmd.EndDeltaY.Should().Be(4);
                cmd.Radius.Should().Be(5);
            });
        }

        [Test]
        public void ClipIndicator_Write_Binary()
        {
            TestCommand(cgm => new ClipIndicator(cgm, true), cmd => cmd.Flag.Should().BeTrue());
            TestCommand(cgm => new ClipIndicator(cgm, false), cmd => cmd.Flag.Should().BeFalse());
        }

        [Test]
        public void ClipInheritance_Write_Binary()
        {
            TestCommand(cgm => new ClipInheritance(cgm, ClipInheritance.Value.INTERSECTION), cmd => cmd.Data.Should().Be(ClipInheritance.Value.INTERSECTION));
            TestCommand(cgm => new ClipInheritance(cgm, ClipInheritance.Value.STLIST), cmd => cmd.Data.Should().Be(ClipInheritance.Value.STLIST));
        }

        [Test]
        public void ClipRectangle_Write_Binary()
        {
            var point = new CGMPoint(2, 2);
            var point2 = new CGMPoint(5, 3);

            TestCommand(cgm => new ClipRectangle(cgm, point, point2), cmd =>
            {
                cmd.Point1.Should().Be(point);
                cmd.Point2.Should().Be(point2);
            });
        }

        [Test]
        public void ColourCalibration_Write_Binary()
        {
            TestCommand(cgm => new ColourCalibration(cgm)
            {
                CalibrationSelection = 2,
                ReferenceX = 2.2,
                ReferenceY = 2.2,
                ReferenceZ = 2.2,
                Xr = 2.2,
                Xg = 2.2,
                Xb = 2.2,
                Yr = 2.2,
                Yg = 2.2,
                Yb = 2.2,
                Zr = 2.2,
                Zg = 2.2,
                Zb = 2.2,
                Ra = 2.2,
                Rb = 2.2,
                Rc = 2.2,
                Ga = 2.2,
                Gb = 2.2,
                Gc = 2.2,
                Ba = 2.2,
                Bb = 2.2,
                Bc = 2.2,
                TableEntries = 1,
                LookupR = new List<Tuple<double, double>>() { new Tuple<double, double>(5, 4) },
                LookupG = new List<Tuple<double, double>>() { new Tuple<double, double>(5, 4) },
                LookupB = new List<Tuple<double, double>>() { new Tuple<double, double>(5, 4) },
                NumberOfGridLocations = 2,
                CmykGridLocations = new List<Color>() { Color.Red, Color.Plum },
                XyzGridLocations = new List<Tuple<double, double, double>>() { new Tuple<double, double, double>(5, 4, 3), new Tuple<double, double, double>(5, 4, 3) }
            }, cmd =>
            {
                cmd.CalibrationSelection.Should().Be(2);
                cmd.ReferenceX.Should().Be(2.1999969482421875);
                cmd.ReferenceY.Should().Be(2.1999969482421875);
                cmd.ReferenceZ.Should().Be(2.1999969482421875);
                cmd.Xr.Should().Be(2.1999969482421875);
                cmd.Xg.Should().Be(2.1999969482421875);
                cmd.Xb.Should().Be(2.1999969482421875);
                cmd.Yr.Should().Be(2.1999969482421875);
                cmd.Yg.Should().Be(2.1999969482421875);
                cmd.Yb.Should().Be(2.1999969482421875);
                cmd.Zr.Should().Be(2.1999969482421875);
                cmd.Zg.Should().Be(2.1999969482421875);
                cmd.Zb.Should().Be(2.1999969482421875);
                cmd.Ra.Should().Be(2.1999969482421875);
                cmd.Rb.Should().Be(2.1999969482421875);
                cmd.Rc.Should().Be(2.1999969482421875);
                cmd.Ga.Should().Be(2.1999969482421875);
                cmd.Gb.Should().Be(2.1999969482421875);
                cmd.Gc.Should().Be(2.1999969482421875);
                cmd.Ba.Should().Be(2.1999969482421875);
                cmd.Bb.Should().Be(2.1999969482421875);
                cmd.Bc.Should().Be(2.1999969482421875);
                cmd.TableEntries.Should().Be(1);
                cmd.LookupR.Should().HaveCount(1);
                cmd.LookupR[0].Should().Be(new Tuple<double, double>(5, 4));
                cmd.LookupG.Should().HaveCount(1);
                cmd.LookupB.Should().HaveCount(1);
                cmd.NumberOfGridLocations.Should().Be(2);
                cmd.CmykGridLocations.Should().HaveCount(2);
                cmd.XyzGridLocations.Should().HaveCount(2);
            });
        }


        [Test]
        public void ColourIndexPrecision_Write_Binary()
        {
            TestCommand(cgm => new ColourIndexPrecision(cgm, 8), cmd => cmd.Precision.Should().Be(8));
        }

        [Test]
        public void ColourModel_Write_Binary()
        {
            TestCommand(cgm => new ColourModel(cgm, ColourModel.Model.RGB), cmd => cmd.Value.Should().Be(ColourModel.Model.RGB));
            TestCommand(cgm => new ColourModel(cgm, ColourModel.Model.CMYK), cmd => cmd.Value.Should().Be(ColourModel.Model.CMYK));
        }

        [Test]
        public void ColourPrecision_Write_Binary()
        {
            TestCommand(cgm => new ColourPrecision(cgm, 8), cmd => cmd.Precision.Should().Be(8));
        }


        [Test]
        public void ColourSelectionMode_Write_Binary()
        {
            TestCommand(cgm => new ColourSelectionMode(cgm, ColourSelectionMode.Type.DIRECT), cmd => cmd.Mode.Should().Be(ColourSelectionMode.Type.DIRECT));
            TestCommand(cgm => new ColourSelectionMode(cgm, ColourSelectionMode.Type.INDEXED), cmd => cmd.Mode.Should().Be(ColourSelectionMode.Type.INDEXED));
        }

        [Test]
        public void ColourTable_Write_Binary()
        {
            TestCommand(cgm => new ColourTable(cgm, 1, new[] { Color.Red, Color.Plum }), cmd =>
            {
                cmd.StartIndex.Should().Be(1);
                cmd.Colors.Should().HaveCount(2);
                cmd.Colors[0].ToArgb().Should().Be(Color.Red.ToArgb());
                cmd.Colors[1].ToArgb().Should().Be(Color.Plum.ToArgb());
            });
            TestCommand(cgm => new ColourTable(cgm, 5, new Color[] { }), cmd =>
            {
                cmd.StartIndex.Should().Be(5);
                cmd.Colors.Should().HaveCount(0);
            });
        }

        [Test]
        public void ColourValueExtent_Write_Binary()
        {
            TestCommand(cgm => new ColourValueExtent(cgm, new[] { 0, 0, 0 }, new[] { 255, 255, 255 }, 2, 0, 0), cmd =>
            {
                cmd.MinimumColorValueRGB.Should().HaveCount(3);
                cmd.MinimumColorValueRGB[0].Should().Be(0);
                cmd.MinimumColorValueRGB[1].Should().Be(0);
                cmd.MinimumColorValueRGB[2].Should().Be(0);
                cmd.MaximumColorValueRGB.Should().HaveCount(3);
                cmd.MaximumColorValueRGB[0].Should().Be(255);
                cmd.MaximumColorValueRGB[1].Should().Be(255);
                cmd.MaximumColorValueRGB[2].Should().Be(255);
                cmd.FirstComponentScale.Should().Be(0);
                cmd.SecondComponentScale.Should().Be(0);
                cmd.ThirdComponentScale.Should().Be(0);
            });

            TestCommand(cgm => new ColourValueExtent(cgm, new[] { 10, 20, 30 }, new[] { 200, 200, 200 }, 0, 0, 0), cmd =>
            {
                cmd.MinimumColorValueRGB.Should().HaveCount(3);
                cmd.MinimumColorValueRGB[0].Should().Be(10);
                cmd.MinimumColorValueRGB[1].Should().Be(20);
                cmd.MinimumColorValueRGB[2].Should().Be(30);
                cmd.MaximumColorValueRGB.Should().HaveCount(3);
                cmd.MaximumColorValueRGB[0].Should().Be(200);
                cmd.MaximumColorValueRGB[1].Should().Be(200);
                cmd.MaximumColorValueRGB[2].Should().Be(200);
                cmd.FirstComponentScale.Should().Be(0);
                cmd.SecondComponentScale.Should().Be(0);
                cmd.ThirdComponentScale.Should().Be(0);
            });

            TestCommand(cgm =>
            {
                cgm.ColourModel = ColourModel.Model.RGB_RELATED;
                cgm.Commands.Add(new ColourModel(cgm, ColourModel.Model.RGB_RELATED));
                return new ColourValueExtent(cgm, new[] { 10, 20, 30 }, new[] { 200, 200, 200 }, 1, 2, 55);
            }, cmd =>
            {
                cmd.MinimumColorValueRGB.Should().HaveCount(0);
                cmd.MaximumColorValueRGB.Should().HaveCount(0);
                cmd.FirstComponentScale.Should().Be(1);
                cmd.SecondComponentScale.Should().Be(2);
                cmd.ThirdComponentScale.Should().Be(55);
            });
        }

        [Test]
        public void ConnectingEdge_Write_Binary()
        {
            TestCommand(cgm => new ConnectingEdge(cgm), cmd => true);
        }

        [Test]
        public void CopySegment_Write_Binary()
        {
            TestCommand(cgm => new CopySegment(cgm, 2, 4, 0, 8, 6, 2, 1, true), cmd =>
            {
                cmd.Id.Should().Be(2);
                cmd.XScale.Should().Be(4);
                cmd.XRotation.Should().Be(0);
                cmd.YRotation.Should().Be(8);
                cmd.YScale.Should().Be(6);
                cmd.XTranslation.Should().Be(2);
                cmd.YTranslation.Should().Be(1);
                cmd.Flag.Should().Be(true);
            });
        }


        [Test]
        public void DeviceViewport_Write_Binary()
        {
            var corner1 = new ViewportPoint() { FirstPoint = new VC() { ValueInt = 6 }, SecondPoint = new VC() { ValueInt = 3 } };
            var corner2 = new ViewportPoint() { FirstPoint = new VC() { ValueInt = 8 }, SecondPoint = new VC() { ValueInt = 1 } };

            TestCommand(cgm =>
            {
                cgm.DeviceViewportSpecificationMode = DeviceViewportSpecificationMode.Mode.MM;
                cgm.Commands.Add(new DeviceViewportSpecificationMode(cgm, DeviceViewportSpecificationMode.Mode.MM, 1));
                return new DeviceViewport(cgm, corner1, corner2);
            }, cmd =>
            {
                cmd.FirstCorner.FirstPoint.ValueInt.Should().Be(6);
                cmd.FirstCorner.SecondPoint.ValueInt.Should().Be(3);
                cmd.SecondCorner.FirstPoint.ValueInt.Should().Be(8);
                cmd.SecondCorner.SecondPoint.ValueInt.Should().Be(1);
            });

            corner1 = new ViewportPoint() { FirstPoint = new VC() { ValueReal = 6 }, SecondPoint = new VC() { ValueReal = 3 } };
            corner2 = new ViewportPoint() { FirstPoint = new VC() { ValueReal = 8 }, SecondPoint = new VC() { ValueReal = 1 } };

            TestCommand(cgm => new DeviceViewport(cgm, corner1, corner2), cmd =>
            {
                cmd.FirstCorner.FirstPoint.ValueReal.Should().Be(6);
                cmd.FirstCorner.SecondPoint.ValueReal.Should().Be(3);
                cmd.SecondCorner.FirstPoint.ValueReal.Should().Be(8);
                cmd.SecondCorner.SecondPoint.ValueReal.Should().Be(1);
            });
        }

        [Test]
        public void DeviceViewportMapping_Write_Binary()
        {
            TestCommand(cgm => new DeviceViewportMapping(cgm, DeviceViewportMapping.Isotropy.FORCED, DeviceViewportMapping.Horizontalalignment.CTR, DeviceViewportMapping.Verticalalignment.CTR), cmd =>
            {
                cmd.IsotropyValue.Should().Be(DeviceViewportMapping.Isotropy.FORCED);
                cmd.HorizontalAlignment.Should().Be(DeviceViewportMapping.Horizontalalignment.CTR);
                cmd.VerticalAlignment.Should().Be(DeviceViewportMapping.Verticalalignment.CTR);
            });

            TestCommand(cgm => new DeviceViewportMapping(cgm, DeviceViewportMapping.Isotropy.NOTFORCED, DeviceViewportMapping.Horizontalalignment.LEFT, DeviceViewportMapping.Verticalalignment.BOTTOM), cmd =>
            {
                cmd.IsotropyValue.Should().Be(DeviceViewportMapping.Isotropy.NOTFORCED);
                cmd.HorizontalAlignment.Should().Be(DeviceViewportMapping.Horizontalalignment.LEFT);
                cmd.VerticalAlignment.Should().Be(DeviceViewportMapping.Verticalalignment.BOTTOM);
            });

        }

        [Test]
        public void DeviceViewportSpecificationMode_Write_Binary()
        {
            TestCommand(cgm => new DeviceViewportSpecificationMode(cgm, DeviceViewportSpecificationMode.Mode.FRACTION, 1), cmd => { cmd.Value.Should().Be(DeviceViewportSpecificationMode.Mode.FRACTION); cmd.MetricScaleFactor.Should().Be(1); });
            TestCommand(cgm => new DeviceViewportSpecificationMode(cgm, DeviceViewportSpecificationMode.Mode.MM, 2), cmd => { cmd.Value.Should().Be(DeviceViewportSpecificationMode.Mode.MM); cmd.MetricScaleFactor.Should().Be(2); });
            TestCommand(cgm => new DeviceViewportSpecificationMode(cgm, DeviceViewportSpecificationMode.Mode.PHYDEVCOORD, 5), cmd => { cmd.Value.Should().Be(DeviceViewportSpecificationMode.Mode.PHYDEVCOORD); cmd.MetricScaleFactor.Should().Be(5); });
        }


        [Test]
        public void DisjointPolyline_Write_Binary()
        {
            TestCommand(cgm => new DisjointPolyline(cgm, new[] { new KeyValuePair<CGMPoint, CGMPoint>(new CGMPoint(1, 2), new CGMPoint(5, 6)) }), cmd =>
            {
                cmd.Lines.Should().HaveCount(1);
                cmd.Lines[0].Key.X.Should().Be(1);
                cmd.Lines[0].Key.Y.Should().Be(2);
                cmd.Lines[0].Value.X.Should().Be(5);
                cmd.Lines[0].Value.Y.Should().Be(6);
            });
        }


        [Test]
        public void EdgeBundleIndex_Write_Binary()
        {
            TestCommand(cgm => new EdgeBundleIndex(cgm, 8), cmd => cmd.Index.Should().Be(8));
        }

        [Test]
        public void EdgeCap_Write_Binary()
        {
            TestCommand(cgm => new EdgeCap(cgm, LineCapIndicator.BUTT, DashCapIndicator.MATCH), cmd =>
            {
                cmd.LineIndicator.Should().Be(LineCapIndicator.BUTT);
                cmd.DashIndicator.Should().Be(DashCapIndicator.MATCH);
            });

            TestCommand(cgm => new EdgeCap(cgm, LineCapIndicator.ROUND, DashCapIndicator.UNSPECIFIED), cmd =>
            {
                cmd.LineIndicator.Should().Be(LineCapIndicator.ROUND);
                cmd.DashIndicator.Should().Be(DashCapIndicator.UNSPECIFIED);
            });
        }

        [Test]
        public void EdgeClipping_Write_Binary()
        {
            TestCommand(cgm => new EdgeClipping(cgm, ClippingMode.LOCUSTHENSHAPE), cmd => cmd.Mode.Should().Be(ClippingMode.LOCUSTHENSHAPE));
        }


        [Test]
        public void EdgeColour_Write_Binary()
        {
            TestCommand(cgm => new EdgeColour(cgm, Color_Index), cmd => IsColorIndex(cmd.Color));
        }

        [Test]
        public void EdgeJoin_Write_Binary()
        {
            TestCommand(cgm => new EdgeJoin(cgm, JoinIndicator.BEVEL), cmd => cmd.Type.Should().Be(JoinIndicator.BEVEL));
        }

        [Test]
        public void EdgeRepresentation_Write_Binary()
        {
            TestCommand(cgm => new EdgeRepresentation(cgm, 2, 3, 5, Color_Index), cmd =>
            {
                cmd.BundleIndex.Should().Be(2);
                cmd.EdgeType.Should().Be(3);
                cmd.EdgeWidth.Should().Be(5);
                cmd.EdgeColor.Should().Be(Color_Index);
            });
        }

        [Test]
        public void EdgeType_Write_Binary()
        {
            TestCommand(cgm => new EdgeType(cgm, DashType.DASH), cmd => cmd.Type.Should().Be(DashType.DASH));
        }

        [Test]
        public void EdgeTypeContinuation_Write_Binary()
        {
            TestCommand(cgm => new EdgeTypeContinuation(cgm, 5), cmd => cmd.Mode.Should().Be(5));
        }

        [Test]
        public void EdgeTypeInitialOffset_Write_Binary()
        {
            TestCommand(cgm => new EdgeTypeInitialOffset(cgm, 5), cmd => cmd.Offset.Should().Be(5));
        }

        [Test]
        public void EdgeVisibility_Write_Binary()
        {
            TestCommand(cgm => new EdgeVisibility(cgm, true), cmd => cmd.IsVisible.Should().Be(true));
            TestCommand(cgm => new EdgeVisibility(cgm, false), cmd => cmd.IsVisible.Should().Be(false));
        }

        [Test]
        public void EdgeWidth_Write_Binary()
        {
            TestCommand(cgm => new EdgeWidth(cgm, 5), cmd => cmd.Width.Should().Be(5));
        }

        [Test]
        public void EdgeWidthSpecificationMode_Write_Binary()
        {
            TestCommand(cgm => new EdgeWidthSpecificationMode(cgm, SpecificationMode.FRACTIONAL), cmd => cmd.Mode.Should().Be(SpecificationMode.FRACTIONAL));
        }

        [Test]
        public void EllipseElement_Write_Binary()
        {
            TestCommand(cgm => new EllipseElement(cgm, Point, Point, Point2), cmd =>
            {
                cmd.Center.Should().Be(Point);
                cmd.FirstConjugateDiameterEndPoint.Should().Be(Point);
                cmd.SecondConjugateDiameterEndPoint.Should().Be(Point2);
            });
        }

        [Test]
        public void EllipticalArc_Write_Binary()
        {
            TestCommand(cgm => new EllipticalArc(cgm, 5, 2, 4, 7, Point, Point, Point2), cmd =>
            {
                cmd.Center.Should().Be(Point);
                cmd.FirstConjugateDiameterEndPoint.Should().Be(Point);
                cmd.SecondConjugateDiameterEndPoint.Should().Be(Point2);
                cmd.StartVectorDeltaX.Should().Be(5);
                cmd.StartVectorDeltaY.Should().Be(2);
                cmd.EndVectorDeltaX.Should().Be(4);
                cmd.EndVectorDeltaY.Should().Be(7);
            });
        }

        [Test]
        public void EllipticalArcClosec_Write_Binary()
        {
            TestCommand(cgm => new EllipticalArcClose(cgm, ClosureType.PIE, 5, 2, 4, 7, Point, Point, Point2), cmd =>
            {
                cmd.ClosureType.Should().Be(ClosureType.PIE);
                cmd.Center.Should().Be(Point);
                cmd.FirstConjugateDiameterEndPoint.Should().Be(Point);
                cmd.SecondConjugateDiameterEndPoint.Should().Be(Point2);
                cmd.StartVectorDeltaX.Should().Be(5);
                cmd.StartVectorDeltaY.Should().Be(2);
                cmd.EndVectorDeltaX.Should().Be(4);
                cmd.EndVectorDeltaY.Should().Be(7);
            });
        }

        [Test]
        public void EndApplicationStructure_Write_Binary()
        {
            TestCommand(cgm => new EndApplicationStructure(cgm), cmd => true);
        }

        [Test]
        public void EndCompoundLine_Write_Binary()
        {
            TestCommand(cgm => new EndCompoundLine(cgm), cmd => true);
        }

        [Test]
        public void EndCompoundTextPath_Write_Binary()
        {
            TestCommand(cgm => new EndCompoundTextPath(cgm), cmd => true);
        }

        [Test]
        public void EndFigure_Write_Binary()
        {
            TestCommand(cgm => new EndFigure(cgm), cmd => true);
        }

        [Test]
        public void EndMetafile_Write_Binary()
        {
            TestCommand(cgm => new EndMetafile(cgm), cmd => true);
        }

        [Test]
        public void EndPicture_Write_Binary()
        {
            TestCommand(cgm => new EndPicture(cgm), cmd => true);
        }

        [Test]
        public void EndProtectionRegion_Write_Binary()
        {
            TestCommand(cgm => new EndProtectionRegion(cgm), cmd => true);
        }

        [Test]
        public void EndSegment_Write_Binary()
        {
            TestCommand(cgm => new EndSegment(cgm), cmd => true);
        }

        [Test]
        public void EndTileArray_Write_Binary()
        {
            TestCommand(cgm => new EndTileArray(cgm), cmd => true);
        }

        [Test]
        public void Escape_Write_Binary()
        {
            TestCommand(cgm => new Escape(cgm, 3, "test1"), cmd =>
            {
                cmd.Identifier.Should().Be(3);
                cmd.DataRecord.Should().Be("test1");
            });
        }

        [Test]
        public void FillBundleIndex_Write_Binary()
        {
            TestCommand(cgm => new FillBundleIndex(cgm, 4), cmd => cmd.Index.Should().Be(4));
        }

        [Test]
        public void FillColour_Write_Binary()
        {
            TestCommand(cgm => new FillColour(cgm, Color_Index), cmd => cmd.Color.Should().Be(Color_Index));
        }

        [Test]
        public void FillReferencePoint_Write_Binary()
        {
            TestCommand(cgm => new FillReferencePoint(cgm, Point), cmd => cmd.Point.Should().Be(Point));
        }

        [Test]
        public void FillRepresentation_Write_Binary()
        {
            TestCommand(cgm => new FillRepresentation(cgm, 3, InteriorStyle.Style.HATCH, Color_Index, 4, 2), cmd =>
            {
                cmd.BundleIndex.Should().Be(3);
                cmd.Style.Should().Be(InteriorStyle.Style.HATCH);
                cmd.Color.Should().Be(Color_Index);
                cmd.HatchIndex.Should().Be(4);
                cmd.PatternIndex.Should().Be(2);
            });
        }

        [Test]
        public void FontList_Write_Binary()
        {
            TestCommand(cgm => new FontList(cgm, new[] { "Arial" }), cmd => {
                cmd.FontNames.Should().HaveCount(1);
                cmd.FontNames[0].Should().Be("Arial");
            });

            TestCommand(cgm => new FontList(cgm, new[] { "Arial", "Arial Bold" }), cmd => {
                cmd.FontNames.Should().HaveCount(2);
                cmd.FontNames[0].Should().Be("Arial");
                cmd.FontNames[1].Should().Be("Arial Bold");
            });
        }

        [Test]
        public void FontProperties_Write_Binary()
        {
            var sdr = new StructuredDataRecord();
            sdr.Add(StructuredDataRecord.StructuredDataType.S, new object[] { "lala" });

            var info = new FontProperties.FontInfo() { Priority = 3, PropertyIndicator = 88, Value = sdr };

            TestCommand(cgm => new FontProperties(cgm, new[] { info }), cmd =>
            {
                cmd.Infos.Should().HaveCount(1);
                cmd.Infos[0].Priority.Should().Be(3);
                cmd.Infos[0].PropertyIndicator.Should().Be(88);
                cmd.Infos[0].Value.Members.Should().HaveCount(1);
                cmd.Infos[0].Value.Members[0].Type.Should().Be(StructuredDataRecord.StructuredDataType.S);
                cmd.Infos[0].Value.Members[0].Data[0].Should().Be("lala");
            });
        }

        [Test]
        public void GeneralizedDrawingPrimitive_Write_Binary()
        {
            TestCommand(cgm => new GeneralizedDrawingPrimitive(cgm, 3, new[] { Point, Point2 }, "test1"), cmd =>
            {
                cmd.Identifier.Should().Be(3);
                cmd.Points.Should().HaveCount(2);
                cmd.Points[0].Should().Be(Point);
                cmd.DataRecord.Should().Be("test1");
            });
        }


        [Test]
        public void GeneralizedTextPathMode_Write_Binary()
        {
            TestCommand(cgm => new GeneralizedTextPathMode(cgm, GeneralizedTextPathMode.TextPathMode.AXIS), cmd => cmd.Mode.Should().Be(GeneralizedTextPathMode.TextPathMode.AXIS));
        }

        [Test]
        public void GeometricPatternDefinition_Write_Binary()
        {
            TestCommand(cgm => new GeometricPatternDefinition(cgm, 3, 5, Point, Point2), cmd =>
            {
                cmd.PatternIndex.Should().Be(3);
                cmd.Identifier.Should().Be(5);
                cmd.FirstCorner.Should().Be(Point);
                cmd.SecondCorner.Should().Be(Point2);
            });
        }

        [Test]
        public void GlyphMapping_Write_Binary()
        {
            var sdr = new StructuredDataRecord();
            sdr.Add(StructuredDataRecord.StructuredDataType.S, new object[] { "lala" });

            var info = new FontProperties.FontInfo() { Priority = 3, PropertyIndicator = 88, Value = sdr };

            TestCommand(cgm => new GlyphMapping(cgm, 2, CharacterSetList.Type.COMPLETE_CODE, "lala", 4, 22, sdr), cmd =>
            {
                cmd.CharacterSetIndex.Should().Be(2);
                cmd.Type.Should().Be(CharacterSetList.Type.COMPLETE_CODE);
                cmd.SequenceTail.Should().Be("lala");
                cmd.OctetsPerCode.Should().Be(4);
                cmd.GlyphSource.Should().Be(22);
                cmd.CodeAssocs.Members.Should().HaveCount(1);
                cmd.CodeAssocs.Members[0].Type.Should().Be(StructuredDataRecord.StructuredDataType.S);
                cmd.CodeAssocs.Members[0].Data[0].Should().Be("lala");
            });
        }

        [Test]
        public void HatchIndex_Write_Binary()
        {
            TestCommand(cgm => new HatchIndex(cgm, HatchIndex.HatchType.HORIZONTAL_VERTICAL_CROSSHATCH), cmd => cmd.Type.Should().Be(HatchIndex.HatchType.HORIZONTAL_VERTICAL_CROSSHATCH));
        }

        [Test]
        public void HatchStyleDefinition_Write_Binary()
        {
            TestCommand(cgm => new HatchStyleDefinition(cgm, 3, HatchStyleDefinition.HatchStyle.CROSSHATCH, 1, 2, 3, 4, 5, new[] { 2, 3 }, new[] { 5, 5 }), cmd =>
            {
                cmd.Index.Should().Be(3);
                cmd.Style.Should().Be(HatchStyleDefinition.HatchStyle.CROSSHATCH);
                cmd.FirstDirX.Should().Be(1);
                cmd.FirstDirY.Should().Be(2);
                cmd.SecondDirX.Should().Be(3);
                cmd.SecondDirY.Should().Be(4);
                cmd.GapWidths.Should().HaveCount(2);
                cmd.GapWidths[0].Should().Be(2);
                cmd.GapWidths[1].Should().Be(3);
                cmd.LineTypes.Should().HaveCount(2);
                cmd.LineTypes[0].Should().Be(5);
                cmd.LineTypes[1].Should().Be(5);
            });
        }

        [Test]
        public void HyperbolicArc_Write_Binary()
        {
            TestCommand(cgm => new HyperbolicArc(cgm, Point, Point2, Point3, 2, 3, 4, 5), cmd =>
            {
                cmd.Center.Should().Be(Point);
                cmd.TransverseRadius.Should().Be(Point2);
                cmd.ConjugateRadius.Should().Be(Point3);
                cmd.StartX.Should().Be(2);
                cmd.StartY.Should().Be(3);
                cmd.EndX.Should().Be(4);
                cmd.EndY.Should().Be(5);
            });
        }

        [Test]
        public void IndexPrecision_Write_Binary()
        {
            TestCommand(cgm => new IndexPrecision(cgm, 16), cmd => cmd.Precision.Should().Be(16));
        }

        [Test]
        public void InheritanceFilter_Write_Binary()
        {
            TestCommand(cgm => new InheritanceFilter(cgm, new[] { InheritanceFilter.Filter.ALLFILL, InheritanceFilter.Filter.MARKERSIZE, InheritanceFilter.Filter.TEXTPATH }, 8), cmd =>
            {
                cmd.Values.Should().HaveCount(3);
                cmd.Values[0].Should().Be(InheritanceFilter.Filter.ALLFILL);
                cmd.Values[1].Should().Be(InheritanceFilter.Filter.MARKERSIZE);
                cmd.Values[2].Should().Be(InheritanceFilter.Filter.TEXTPATH);
                cmd.Setting.Should().Be(8);
            });
        }

        [Test]
        public void IntegerPrecision_Write_Binary()
        {
            TestCommand(cgm => new IntegerPrecision(cgm, 16), cmd => cmd.Precision.Should().Be(16));
        }

        [Test]
        public void InteriorStyle_Write_Binary()
        {
            TestCommand(cgm => new InteriorStyle(cgm, InteriorStyle.Style.HATCH), cmd => cmd.Value.Should().Be(InteriorStyle.Style.HATCH));
        }

        [Test]
        public void InteriorStyleSpecificationMode_Write_Binary()
        {
            TestCommand(cgm => new InteriorStyleSpecificationMode(cgm, SpecificationMode.FRACTIONAL), cmd => cmd.Mode.Should().Be(SpecificationMode.FRACTIONAL));
        }

        [Test]
        public void InterpolatedInterior_Write_Binary()
        {
            TestCommand(cgm => new InterpolatedInterior(cgm, 2, new[] { 2.0, 4 }, new[] { 7.0, 5 }, new[] { 44.0, 3 }, new[] { Color_Index, Color_Index2, Color_Index }), cmd =>
            {
                cmd.Style.Should().Be(2);
                cmd.GeoX.Should().HaveCount(2);
                cmd.GeoX[0].Should().Be(2);
                cmd.GeoX[1].Should().Be(4);
                cmd.GeoY.Should().HaveCount(2);
                cmd.GeoY[0].Should().Be(7);
                cmd.GeoY[1].Should().Be(5);
                cmd.StageDesignators.Should().HaveCount(2);
                cmd.StageDesignators[0].Should().Be(44);
                cmd.StageDesignators[1].Should().Be(3);
                cmd.Colors.Should().HaveCount(3);
                cmd.Colors[0].Should().Be(Color_Index);
                cmd.Colors[1].Should().Be(Color_Index2);
                cmd.Colors[2].Should().Be(Color_Index);
            });
        }

        [Test]
        public void LineAndEdgeTypeDefinition_Write_Binary()
        {
            TestCommand(cgm => new LineAndEdgeTypeDefinition(cgm, -2, 4, new[] { 5, 8, 3 }), cmd =>
            {
                cmd.LineType.Should().Be(-2);
                cmd.DashCycleRepeatLength.Should().Be(4);
                cmd.DashElements.Should().HaveCount(3);
                cmd.DashElements[0].Should().Be(5);
                cmd.DashElements[1].Should().Be(8);
                cmd.DashElements[2].Should().Be(3);
            });
        }

        [Test]
        public void LineBundleIndex_Write_Binary()
        {
            TestCommand(cgm => new LineBundleIndex(cgm, 3), cmd => cmd.Index.Should().Be(3));
        }

        [Test]
        public void LineCap_Write_Binary()
        {
            TestCommand(cgm => new LineCap(cgm, LineCapIndicator.BUTT, DashCapIndicator.MATCH), cmd =>
            {
                cmd.LineIndicator.Should().Be(LineCapIndicator.BUTT);
                cmd.DashIndicator.Should().Be(DashCapIndicator.MATCH);
            });
        }

        [Test]
        public void LineClipping_Write_Binary()
        {
            TestCommand(cgm => new LineClipping(cgm, ClippingMode.LOCUS), cmd => cmd.Mode.Should().Be(ClippingMode.LOCUS));
        }

        [Test]
        public void LineColour_Write_Binary()
        {
            TestCommand(cgm => new LineColour(cgm, Color_Index), cmd => cmd.Color.Should().Be(Color_Index));
        }

        [Test]
        public void LineJoin_Write_Binary()
        {
            TestCommand(cgm => new LineJoin(cgm, JoinIndicator.BEVEL), cmd => cmd.Type.Should().Be(JoinIndicator.BEVEL));
        }

        [Test]
        public void LineRepresentation_Write_Binary()
        {
            TestCommand(cgm => new LineRepresentation(cgm, 2, 5, 6, Color_Index), cmd =>
            {
                cmd.Index.Should().Be(2);
                cmd.LineType.Should().Be(5);
                cmd.LineWidth.Should().Be(6);
                cmd.Color.Should().Be(Color_Index);
            });
        }

        [Test]
        public void LineType_Write_Binary()
        {
            TestCommand(cgm => new LineType(cgm, DashType.DASH_DOT), cmd => cmd.Type.Should().Be(DashType.DASH_DOT));
        }

        [Test]
        public void LineTypeContinuation_Write_Binary()
        {
            TestCommand(cgm => new LineTypeContinuation(cgm, 3), cmd => cmd.Mode.Should().Be(3));
        }

        [Test]
        public void LineTypeInitialOffset_Write_Binary()
        {
            TestCommand(cgm => new LineTypeInitialOffset(cgm, 3), cmd => cmd.Offset.Should().Be(3));
        }

        [Test]
        public void LineWidth_Write_Binary()
        {
            TestCommand(cgm => new LineWidth(cgm, 5), cmd => cmd.Width.Should().Be(5));
        }

        [Test]
        public void LineWidthSpecificationMode_Write_Binary()
        {
            TestCommand(cgm => new LineWidthSpecificationMode(cgm, SpecificationMode.MM), cmd => cmd.Mode.Should().Be(SpecificationMode.MM));
        }

        [Test]
        public void MarkerBundleIndex_Write_Binary()
        {
            TestCommand(cgm => new MarkerBundleIndex(cgm, 2), cmd => cmd.Index.Should().Be(2));
        }

        [Test]
        public void MarkerClipping_Write_Binary()
        {
            TestCommand(cgm => new MarkerClipping(cgm, ClippingMode.LOCUSTHENSHAPE), cmd => cmd.Mode.Should().Be(ClippingMode.LOCUSTHENSHAPE));
        }

        [Test]
        public void MarkerColour_Write_Binary()
        {
            TestCommand(cgm => new MarkerColour(cgm, Color_Index), cmd => cmd.Color.Should().Be(Color_Index));
        }

        [Test]
        public void MarkerRepresentation_Write_Binary()
        {
            TestCommand(cgm => new MarkerRepresentation(cgm, 2, 5, 6, Color_Index), cmd =>
            {
                cmd.Index.Should().Be(2);
                cmd.Type.Should().Be(5);
                cmd.Size.Should().Be(6);
                cmd.Color.Should().Be(Color_Index);
            });
        }

        [Test]
        public void MarkerSize_Write_Binary()
        {
            TestCommand(cgm => new MarkerSize(cgm, 2), cmd => cmd.Width.Should().Be(2));
        }

        [Test]
        public void MarkerSizeSpecificationMode_Write_Binary()
        {
            TestCommand(cgm => new MarkerSizeSpecificationMode(cgm, SpecificationMode.SCALED), cmd => cmd.Mode.Should().Be(SpecificationMode.SCALED));
        }

        [Test]
        public void MarkerType_Write_Binary()
        {
            TestCommand(cgm => new MarkerType(cgm, MarkerType.Type.CIRCLE), cmd => cmd.Value.Should().Be(MarkerType.Type.CIRCLE));
        }

        [Test]
        public void MaximumColourIndex_Write_Binary()
        {
            TestCommand(cgm => new MaximumColourIndex(cgm, 240), cmd => cmd.Value.Should().Be(240));
        }

        [Test]
        public void MaximumVDCExtent_Write_Binary()
        {
            TestCommand(cgm => new MaximumVDCExtent(cgm, Point, Point2), cmd =>
            {
                cmd.FirstCorner.Should().Be(Point);
                cmd.SecondCorner.Should().Be(Point2);
            });
        }

        [Test]
        public void MessageCommand_Write_Binary()
        {
            TestCommand(cgm => new MessageCommand(cgm, MessageCommand.ActionType.Action, "testtt"), cmd =>
            {
                cmd.Action.Should().Be(MessageCommand.ActionType.Action);
                cmd.Message.Should().Be("testtt");
            });
        }

        [Test]
        public void MetafileDefaultsReplacement_Write_Binary()
        {
            TestCommand(cgm =>
            {
                var command = new MaximumColourIndex(cgm, 55);
                return new MetafileDefaultsReplacement(cgm, command);
            }, cmd =>
            {
                cmd.EmbeddedCommand.Should().NotBeNull();
                cmd.EmbeddedCommand.ElementClass.Should().Be(ClassCode.MetafileDescriptorElements);
                cmd.EmbeddedCommand.ElementId.Should().Be(9);
                cmd.EmbeddedCommand.Should().BeOfType<MaximumColourIndex>();
                (cmd.EmbeddedCommand as MaximumColourIndex).Value.Should().Be(55);
            });
        }

        [Test]
        public void MetafileDescription_Write_Binary()
        {
            TestCommand(cgm => new MetafileDescription(cgm, "test"), cmd => cmd.Description.Should().Be("test"));
            TestCommand(cgm => new MetafileDescription(cgm, "tes".PadRight(300)), cmd => cmd.Description.Should().Be("tes".PadRight(300)));
        }

        [Test]
        public void MetafileElementList_Write_Binary()
        {
            TestCommand(cgm => new MetafileElementList(cgm, MetafileElementList.DRAWINGPLUS), cmd => cmd.Elements[0] == MetafileElementList.DRAWINGPLUS);
            TestCommand(cgm => new MetafileElementList(cgm, " (1,5)"), cmd => cmd.Elements[0] == " (1,5)");
        }

        [Test]
        public void MetafileVersion_Write_Binary()
        {
            TestCommand(cgm => new MetafileVersion(cgm, 1), cmd => cmd.Version.Should().Be(1));
            TestCommand(cgm => new MetafileVersion(cgm, 3), cmd => cmd.Version.Should().Be(3));
        }

        [Test]
        public void MitreLimit_Write_Binary()
        {
            TestCommand(cgm => new MitreLimit(cgm, 5), cmd => cmd.Limit.Should().Be(5));
        }

        [Test]
        public void NamePrecision_Write_Binary()
        {
            TestCommand(cgm => new NamePrecision(cgm, 8), cmd => cmd.Precision.Should().Be(8));
        }

        [Test]
        public void NewRegion_Write_Binary()
        {
            TestCommand(cgm => new NewRegion(cgm), cmd => true);
        }

        [Test]
        public void NonUniformBSpline_Write_Binary()
        {
            TestCommand(cgm => new NonUniformBSpline(cgm, 2, new[] { Point, Point2 }, new[] { 4.0, 6, 8, 33 }, 4, 5), cmd =>
                 {
                     cmd.SplineOrder.Should().Be(2);
                     cmd.Points.Should().HaveCount(2);
                     cmd.Points[0].Should().Be(Point);
                     cmd.Points[1].Should().Be(Point2);
                     cmd.Knots.Should().HaveCount(4);
                     cmd.Knots[0].Should().Be(4);
                     cmd.Knots[1].Should().Be(6);
                     cmd.Knots[2].Should().Be(8);
                     cmd.Knots[3].Should().Be(33);
                     cmd.StartValue.Should().Be(4);
                     cmd.EndValue.Should().Be(5);
                 });
        }

        [Test]
        public void NonUniformRationalBSpline_Write_Binary()
        {
            TestCommand(cgm => new NonUniformRationalBSpline(cgm, 2, new[] { Point, Point2 }, new[] { 4.0, 6, 8, 33 }, 4, 5, new[] { 8.0, 6 }), cmd =>
             {
                 cmd.SplineOrder.Should().Be(2);
                 cmd.Points.Should().HaveCount(2);
                 cmd.Points[0].Should().Be(Point);
                 cmd.Points[1].Should().Be(Point2);
                 cmd.Knots.Should().HaveCount(4);
                 cmd.Knots[0].Should().Be(4);
                 cmd.Knots[1].Should().Be(6);
                 cmd.Knots[2].Should().Be(8);
                 cmd.Knots[3].Should().Be(33);
                 cmd.StartValue.Should().Be(4);
                 cmd.EndValue.Should().Be(5);
                 cmd.Weights.Should().HaveCount(2);
                 cmd.Weights[0].Should().Be(8);
                 cmd.Weights[1].Should().Be(6);
             });
        }

        [Test]
        public void NoOp_Write_Binary()
        {
            TestCommand(cgm => new NoOp(cgm), cmd => true);
        }

        [Test]
        public void ParabolicArc_Write_Binary()
        {
            TestCommand(cgm => new ParabolicArc(cgm, Point, Point2, Point3), cmd =>
           {
               cmd.IntersectionPoint.Should().Be(Point);
               cmd.Start.Should().Be(Point2);
               cmd.End.Should().Be(Point3);
           });
        }

        [Test]
        public void PatternIndex_Write_Binary()
        {
            TestCommand(cgm => new PatternIndex(cgm, 8), cmd => cmd.Index.Should().Be(8));
        }

        [Test]
        public void PatternSize_Write_Binary()
        {
            TestCommand(cgm => new PatternSize(cgm, 3, 4, 5, 6), cmd =>
               {
                   cmd.HeightX.Should().Be(3);
                   cmd.HeightY.Should().Be(4);
                   cmd.WidthX.Should().Be(5);
                   cmd.WidthY.Should().Be(6);
               });
        }

        [Test]
        public void PatternTable_Write_Binary()
        {
            TestCommand(cgm => new PatternTable(cgm, 3, 2, 1, 8, new[] { Color_Index, Color_Index2 }), cmd =>
              {
                  cmd.Index.Should().Be(3);
                  cmd.Nx.Should().Be(2);
                  cmd.Ny.Should().Be(1);
                  cmd.Colors.Should().HaveCount(2);
                  cmd.Colors[0].Should().Be(Color_Index);
                  cmd.Colors[1].Should().Be(Color_Index2);
              });
        }

        [Test]
        public void PickIdentifier_Write_Binary()
        {
            TestCommand(cgm => new PickIdentifier(cgm, 8), cmd => cmd.Identifier.Should().Be(8));
        }

        [Test]
        public void PictureDirectory_Write_Binary()
        {
            var info = new PictureDirectory.PDInfo() { Identifier = "aa", Directory = 2, Location = 5 };
            var info2 = new PictureDirectory.PDInfo() { Identifier = "bbbb", Directory = 5, Location = 66 };

            TestCommand(cgm => new PictureDirectory(cgm, PictureDirectory.Type.UI32, new[] { info, info2 }), cmd =>
             {
                 cmd.Value.Should().Be(PictureDirectory.Type.UI32);
                 cmd.Infos.Should().HaveCount(2);
                 cmd.Infos[0].Identifier.Should().Be(info.Identifier);
                 cmd.Infos[0].Directory.Should().Be(info.Directory);
                 cmd.Infos[0].Location.Should().Be(info.Location);
                 cmd.Infos[1].Identifier.Should().Be(info2.Identifier);
                 cmd.Infos[1].Directory.Should().Be(info2.Directory);
                 cmd.Infos[1].Location.Should().Be(info2.Location);
             });
        }

        [Test]
        public void PolyBezier_Write_Binary_ContinuityIndicator_1()
        {
            var bezier = new BezierCurve() { Point, Point2, Point3, Point2 };
            var bezier2 = new BezierCurve() { Point2, Point, Point, Point3 };
            var bezier3 = new BezierCurve() { Point, Point, Point, Point2 };


            TestCommand(cgm => new PolyBezier(cgm, 1, new[] { bezier, bezier2, bezier3 }), cmd =>
           {
               cmd.ContinuityIndicator.Should().Be(1);
               cmd.Curves.Should().HaveCount(3);

               cmd.Curves[0].Should().HaveCount(4);
               cmd.Curves[0][0].Should().Be(Point);
               cmd.Curves[0][1].Should().Be(Point2);
               cmd.Curves[0][2].Should().Be(Point3);
               cmd.Curves[0][3].Should().Be(Point2);

               cmd.Curves[1].Should().HaveCount(4);
               cmd.Curves[1][0].Should().Be(Point2);
               cmd.Curves[1][1].Should().Be(Point);
               cmd.Curves[1][2].Should().Be(Point);
               cmd.Curves[1][3].Should().Be(Point3);

               cmd.Curves[2].Should().HaveCount(4);
               cmd.Curves[2][0].Should().Be(Point);
               cmd.Curves[2][1].Should().Be(Point);
               cmd.Curves[2][2].Should().Be(Point);
               cmd.Curves[2][3].Should().Be(Point2);
           });
        }

        [Test]
        public void PolyBezier_Write_Binary_ContinuityIndicator_2()
        {
            var bezier = new BezierCurve() { Point, Point2, Point3, Point2 };
            var bezier2 = new BezierCurve() { Point2, Point, Point };
            var bezier3 = new BezierCurve() { Point, Point, Point };
            var bezier4 = new BezierCurve() { Point3, Point2, Point2 };

            TestCommand(cgm => new PolyBezier(cgm, 2, new[] { bezier, bezier2, bezier3, bezier4 }), cmd =>
            {
                cmd.ContinuityIndicator.Should().Be(2);
                cmd.Curves.Should().HaveCount(4);
                cmd.Curves[0].Should().HaveCount(4);
                cmd.Curves[0][0].Should().Be(Point);
                cmd.Curves[0][1].Should().Be(Point2);
                cmd.Curves[0][2].Should().Be(Point3);
                cmd.Curves[0][3].Should().Be(Point2);

                cmd.Curves[1].Should().HaveCount(3);
                cmd.Curves[1][0].Should().Be(Point2);
                cmd.Curves[1][1].Should().Be(Point);
                cmd.Curves[1][2].Should().Be(Point);

                cmd.Curves[2].Should().HaveCount(3);
                cmd.Curves[2][0].Should().Be(Point);
                cmd.Curves[2][1].Should().Be(Point);
                cmd.Curves[2][2].Should().Be(Point);

                cmd.Curves[3].Should().HaveCount(3);
                cmd.Curves[3][0].Should().Be(Point3);
                cmd.Curves[3][1].Should().Be(Point2);
                cmd.Curves[3][2].Should().Be(Point2);
            });
        }

        [Test]
        public void PolygonElement_Write_Binary()
        {
            TestCommand(cgm => new PolygonElement(cgm, new[] { Point, Point2, Point2, Point3 }), cmd =>
            {
                cmd.Points.Should().HaveCount(4);
                cmd.Points[0].Should().Be(Point);
                cmd.Points[1].Should().Be(Point2);
                cmd.Points[2].Should().Be(Point2);
                cmd.Points[3].Should().Be(Point3);
            });
        }

        [Test]
        public void PolygonSet_Write_Binary()
        {
            TestCommand(cgm => new PolygonSet(cgm, new[] { new KeyValuePair<PolygonSet.EdgeFlag, CGMPoint>(PolygonSet.EdgeFlag.CLOSEVIS, Point), new KeyValuePair<PolygonSet.EdgeFlag, CGMPoint>(PolygonSet.EdgeFlag.CLOSEINVIS, Point2) }), cmd =>
           {
               cmd.Set.Should().HaveCount(2);
               cmd.Set[0].Key.Should().Be(PolygonSet.EdgeFlag.CLOSEVIS);
               cmd.Set[0].Value.Should().Be(Point);
               cmd.Set[1].Key.Should().Be(PolygonSet.EdgeFlag.CLOSEINVIS);
               cmd.Set[1].Value.Should().Be(Point2);
           });
        }

        [Test]
        public void Polyline_Write_Binary()
        {
            TestCommand(cgm => new Polyline(cgm, new[] { Point, Point2, Point2, Point3 }), cmd =>
            {
                cmd.Points.Should().HaveCount(4);
                cmd.Points[0].Should().Be(Point);
                cmd.Points[1].Should().Be(Point2);
                cmd.Points[2].Should().Be(Point2);
                cmd.Points[3].Should().Be(Point3);
            });
        }

        [Test]
        public void PolyMarker_Write_Binary()
        {
            TestCommand(cgm => new PolyMarker(cgm, new[] { Point, Point2, Point2, Point3 }), cmd =>
            {
                cmd.Points.Should().HaveCount(4);
                cmd.Points[0].Should().Be(Point);
                cmd.Points[1].Should().Be(Point2);
                cmd.Points[2].Should().Be(Point2);
                cmd.Points[3].Should().Be(Point3);
            });
        }

        [Test]
        public void PolySymbol_Write_Binary()
        {
            TestCommand(cgm => new PolySymbol(cgm, 4, new[] { Point, Point2, Point2, Point3 }), cmd =>
            {
                cmd.Index.Should().Be(4);
                cmd.Points.Should().HaveCount(4);
                cmd.Points[0].Should().Be(Point);
                cmd.Points[1].Should().Be(Point2);
                cmd.Points[2].Should().Be(Point2);
                cmd.Points[3].Should().Be(Point3);
            });
        }

        [Test]
        public void ProtectionRegionIndicator_Write_Binary()
        {
            TestCommand(cgm => new ProtectionRegionIndicator(cgm, 4, 6), cmd =>
            {
                cmd.Index.Should().Be(4);
                cmd.Indicator.Should().Be(6);
            });
        }

        [Test]
        public void RealPrecision_Write_Binary()
        {
            TestCommand(cgm => new RealPrecision(cgm, Precision.Fixed_32), cmd => cmd.Value == Precision.Fixed_32);
            TestCommand(cgm => new RealPrecision(cgm, Precision.Fixed_64), cmd => cmd.Value == Precision.Fixed_64);
            TestCommand(cgm => new RealPrecision(cgm, Precision.Floating_32), cmd => cmd.Value == Precision.Floating_32);
            TestCommand(cgm => new RealPrecision(cgm, Precision.Floating_64), cmd => cmd.Value == Precision.Floating_64);
        }

        [Test]
        public void RectangleElement_Write_Binary()
        {
            TestCommand(cgm => new RectangleElement(cgm, Point2, Point3), cmd =>
            {
                cmd.FirstCorner.Should().Be(Point2);
                cmd.SecondCorner.Should().Be(Point3);
            });
        }

        [Test]
        public void RestorePrimitiveContext_Write_Binary()
        {
            TestCommand(cgm => new RestorePrimitiveContext(cgm, 16), cmd => cmd.Name.Should().Be(16));
        }

        [Test]
        public void RestrictedText_Write_Binary()
        {
            TestCommand(cgm => new RestrictedText(cgm, "testdata", Point, 2, 5, false), cmd =>
            {
                cmd.Text.Should().Be("testdata");
                cmd.Position.Should().Be(Point);
                cmd.DeltaWidth.Should().Be(2);
                cmd.DeltaHeight.Should().Be(5);
                cmd.Final.Should().Be(false);
            });
        }

        [Test]
        public void RestrictedTextType_Write_Binary()
        {
            TestCommand(cgm => new RestrictedTextType(cgm, RestrictedTextType.Type.BOXED_ALL), cmd => cmd.Value.Should().Be(RestrictedTextType.Type.BOXED_ALL));
        }

        [Test]
        public void SavePrimitiveContext_Write_Binary()
        {
            TestCommand(cgm => new SavePrimitiveContext(cgm, 11), cmd => cmd.Name.Should().Be(11));
        }

        [Test]
        public void ScalingMode_Write_Binary_Abstract()
        {
            TestCommand(cgm => new ScalingMode(cgm, ScalingMode.Mode.ABSTRACT, 5), cmd =>
            {
                cmd.Value.Should().Be(ScalingMode.Mode.ABSTRACT);
                cmd.MetricScalingFactor.Should().Be(0);
            });
        }

        [Test]
        public void ScalingMode_Write_Binary_Metric()
        {
            TestCommand(cgm => new ScalingMode(cgm, ScalingMode.Mode.METRIC, 5), cmd =>
            {
                cmd.Value.Should().Be(ScalingMode.Mode.METRIC);
                cmd.MetricScalingFactor.Should().Be(5);
            });
        }

        [Test]
        public void SegmentDisplayPriority_Write_Binary()
        {
            TestCommand(cgm => new SegmentDisplayPriority(cgm, 33, 5), cmd =>
            {
                cmd.Name.Should().Be(33);
                cmd.Prio.Should().Be(5);
            });
        }

        [Test]
        public void SegmentHighlighting_Write_Binary()
        {
            TestCommand(cgm => new SegmentHighlighting(cgm, 33, SegmentHighlighting.Highlighting.HIGHL), cmd =>
            {
                cmd.Identifier.Should().Be(33);
                cmd.Value.Should().Be(SegmentHighlighting.Highlighting.HIGHL);
            });
        }

        [Test]
        public void SegmentPickPriority_Write_Binary()
        {
            TestCommand(cgm => new SegmentPickPriority(cgm, 33, 5), cmd =>
            {
                cmd.Identifier.Should().Be(33);
                cmd.Prio.Should().Be(5);
            });
        }

        [Test]
        public void SegmentPriorityExtend_Write_Binary()
        {
            TestCommand(cgm => new SegmentPriorityExtend(cgm, 33, 5), cmd =>
            {
                cmd.Min.Should().Be(33);
                cmd.Max.Should().Be(5);
            });
        }

        [Test]
        public void SegmentTransformation_Write_Binary()
        {
            TestCommand(cgm => new SegmentTransformation(cgm, 33, 1, 3, 66, 2, 45, 8), cmd =>
            {
                cmd.Identifier.Should().Be(33);
                cmd.ScaleX.Should().Be(1);
                cmd.RotationX.Should().Be(3);
                cmd.RotationY.Should().Be(66);
                cmd.ScaleY.Should().Be(2);
                cmd.TranslationX.Should().Be(45);
                cmd.TranslationY.Should().Be(8);
            });
        }

        [Test]
        public void SymbolColour_Write_Binary()
        {
            TestCommand(cgm => new SymbolColour(cgm, Color_Index), cmd => cmd.Color.Should().Be(Color_Index));
        }

        [Test]
        public void SymbolLibraryIndex_Write_Binary()
        {
            TestCommand(cgm => new SymbolLibraryIndex(cgm, 33), cmd => cmd.Index.Should().Be(33));
        }

        [Test]
        public void SymbolLibraryList_Write_Binary()
        {
            TestCommand(cgm => new SymbolLibraryList(cgm, new[] { "test1", "another test" }), cmd =>
            {
                cmd.Names.Should().HaveCount(2);
                cmd.Names[0].Should().Be("test1");
                cmd.Names[1].Should().Be("another test");
            });
        }

        [Test]
        public void SymbolOrientation_Write_Binary()
        {
            TestCommand(cgm => new SymbolOrientation(cgm, 4, 7, 33, 1), cmd =>
            {
                cmd.UpX.Should().Be(4);
                cmd.UpY.Should().Be(7);
                cmd.BaseX.Should().Be(33);
                cmd.BaseY.Should().Be(1);
            });
        }

        [Test]
        public void SymbolSize_Write_Binary()
        {
            TestCommand(cgm => new SymbolSize(cgm, SymbolSize.ScaleIndicator.BOTH, 3, 6), cmd =>
            {
                cmd.Indicator.Should().Be(SymbolSize.ScaleIndicator.BOTH);
                cmd.Width.Should().Be(3);
                cmd.Height.Should().Be(6);
            });
        }

        [Test]
        public void Text_Write_Binary()
        {
            TestCommand(cgm => new Text(cgm, "this is a test", Point, true), cmd =>
            {
                cmd.Text.Should().Be("this is a test");
                cmd.Position.Should().Be(Point);
                cmd.Final.Should().Be(true);
            });
        }

        [Test]
        public void TextAlignment_Write_Binary()
        {
            TestCommand(cgm => new TextAlignment(cgm, TextAlignment.HorizontalAlignmentType.LEFT, TextAlignment.VerticalAlignmentType.BOTTOM, 3, 6), cmd =>
            {
                cmd.HorizontalAlignment.Should().Be(TextAlignment.HorizontalAlignmentType.LEFT);
                cmd.VerticalAlignment.Should().Be(TextAlignment.VerticalAlignmentType.BOTTOM);
                cmd.ContinuousHorizontalAlignment.Should().Be(3);
                cmd.ContinuousVerticalAlignment.Should().Be(6);
            });
        }

        [Test]
        public void TextBundleIndex_Write_Binary()
        {
            TestCommand(cgm => new TextBundleIndex(cgm, 16), cmd => cmd.Index.Should().Be(16));
        }

        [Test]
        public void TextColour_Write_Binary()
        {
            TestCommand(cgm => new TextColour(cgm, Color_Index2), cmd => cmd.Color.Should().Be(Color_Index2));
        }

        [Test]
        public void TextFontIndex_Write_Binary()
        {
            TestCommand(cgm => new TextFontIndex(cgm, 23), cmd => cmd.Index.Should().Be(23));
        }

        [Test]
        public void TextPath_Write_Binary()
        {
            TestCommand(cgm => new TextPath(cgm, TextPath.Type.LEFT), cmd => cmd.Path.Should().Be(TextPath.Type.LEFT));
        }

        [Test]
        public void TextPrecision_Write_Binary()
        {
            TestCommand(cgm => new TextPrecision(cgm, TextPrecisionType.CHAR), cmd => cmd.Value.Should().Be(TextPrecisionType.CHAR));
            TestCommand(cgm => new TextPrecision(cgm, TextPrecisionType.STRING), cmd => cmd.Value.Should().Be(TextPrecisionType.STRING));
        }

        [Test]
        public void TextRepresentation_Write_Binary()
        {
            TestCommand(cgm => new TextRepresentation(cgm, 2, 5, TextPrecisionType.STRING, 44, 7, Color_Index), cmd =>
            {
                cmd.BundleIndex.Should().Be(2);
                cmd.FontIndex.Should().Be(5);
                cmd.Precision.Should().Be(TextPrecisionType.STRING);
                cmd.Spacing.Should().Be(44);
                cmd.Expansion.Should().Be(7);
                cmd.Color.Should().Be(Color_Index);
            });
        }

        [Test]
        public void TextScoreType_Write_Binary()
        {
            TestCommand(cgm => new TextScoreType(cgm, new[] { new TextScoreType.TSInfo() { Type = 5, Indicator = true }, new TextScoreType.TSInfo() { Type = 2, Indicator = false } }), cmd =>
             {
                 cmd.Infos.Should().HaveCount(2);
                 cmd.Infos[0].Type.Should().Be(5);
                 cmd.Infos[0].Indicator.Should().Be(true);
                 cmd.Infos[1].Type.Should().Be(2);
                 cmd.Infos[1].Indicator.Should().Be(false);
             });
        }

        [Test]
        public void Tile_Write_Binary()
        {
            var sdr = new StructuredDataRecord();
            sdr.Add(StructuredDataRecord.StructuredDataType.E, new object[] { 2 });
            sdr.Add(StructuredDataRecord.StructuredDataType.IX, new object[] { 5,6 });
            var image = new MemoryStream(new byte[] { 1, 20, 30, 5, 45 });

            TestCommand(cgm => new Tile(cgm, CompressionType.BITMAP, 1, 8, sdr, image), cmd =>
            {
                cmd.CompressionType.Should().Be(CompressionType.BITMAP);
                cmd.RowPaddingIndicator.Should().Be(1);
                cmd.CellColorPrecision.Should().Be(8);
                cmd.DataRecord.Members.Should().HaveCount(2);
                cmd.DataRecord.Members[0].Type.Should().Be(StructuredDataRecord.StructuredDataType.E);
                cmd.DataRecord.Members[0].Count.Should().Be(1);
                cmd.DataRecord.Members[0].Data[0].Should().Be(2);
                cmd.DataRecord.Members[1].Type.Should().Be(StructuredDataRecord.StructuredDataType.IX);
                cmd.DataRecord.Members[1].Count.Should().Be(2);
                cmd.DataRecord.Members[1].Data[0].Should().Be(5);
                cmd.DataRecord.Members[1].Data[1].Should().Be(6);
            });

            TestCommand(cgm => new Tile(cgm, CompressionType.PNG, 88, 16, sdr, image), cmd =>
            {
                cmd.CompressionType.Should().Be(CompressionType.PNG);
                cmd.RowPaddingIndicator.Should().Be(88);
                cmd.CellColorPrecision.Should().Be(16);
                cmd.DataRecord.Members.Should().HaveCount(2);
                cmd.DataRecord.Members[0].Type.Should().Be(StructuredDataRecord.StructuredDataType.E);
                cmd.DataRecord.Members[1].Type.Should().Be(StructuredDataRecord.StructuredDataType.IX);
                cmd.DataRecord.Members[1].Count.Should().Be(2);
                cmd.DataRecord.Members[1].Data[0].Should().Be(5);
                cmd.DataRecord.Members[1].Data[1].Should().Be(6);
                cmd.Image.ToArray().Should().ContainInOrder(image.ToArray());
            });
        }

        [Test]
        public void Transparency_Write_Binary()
        {
            TestCommand(cgm => new Transparency(cgm, true), cmd => cmd.Flag.Should().Be(true));
        }

        [Test]
        public void TransparentCellColour_Write_Binary()
        {
            TestCommand(cgm => new TransparentCellColour(cgm, true, Color_Index), cmd =>
            {
                cmd.Indicator.Should().Be(true);
                cmd.Color.Should().Be(Color_Index);
            });
        }

        [Test]
        public void VDCExtent_Write_Binary()
        {
            TestCommand(cgm => new VDCExtent(cgm, Point, Point2), cmd =>
            {
                cmd.LowerLeftCorner.Should().Be(Point);
                cmd.UpperRightCorner.Should().Be(Point2);
            });
        }

        [Test]
        public void VDCExtent_Write_Binary_Negative()
        {
            var negativePoint = new CGMPoint(0, -0.4363);

            TestCommand(cgm => {
                cgm.Commands.Add(new VDCType(cgm, VDCType.Type.Real));
                return new VDCExtent(cgm, negativePoint, Point2);
            }, cmd =>
            {
                cmd.LowerLeftCorner.Should().Be(negativePoint);
                cmd.UpperRightCorner.Should().Be(Point2);
            });           
        }

        [Test]
        public void VDCExtent_Write_Binary_Negative2()
        {           
            var negativePoint = new CGMPoint(-1.0000, -5.0825);

            TestCommand(cgm => {
                cgm.Commands.Add(new VDCType(cgm, VDCType.Type.Real));
                return new VDCExtent(cgm, negativePoint, Point2);
            }, cmd =>
            {
                cmd.LowerLeftCorner.Should().Be(negativePoint);
                cmd.UpperRightCorner.Should().Be(Point2);
            });
        }

        [Test]
        public void VDCExtent_Write_Binary_Negative3()
        {
            var negativePoint = new CGMPoint(0.0000, -5.0);

            TestCommand(cgm => {
                cgm.Commands.Add(new VDCType(cgm, VDCType.Type.Real));
                return new VDCExtent(cgm, negativePoint, Point2);
            }, cmd =>
            {
                cmd.LowerLeftCorner.Should().Be(negativePoint);
                cmd.UpperRightCorner.Should().Be(Point2);
            });
        }

        [Test]
        public void VDCIntegerPrecision_Write_Binary()
        {
            TestCommand(cgm => new VDCIntegerPrecision(cgm, 16), cmd => cmd.Precision.Should().Be(16));
            TestCommand(cgm => new VDCIntegerPrecision(cgm, 24), cmd => cmd.Precision.Should().Be(24));
        }

        [Test]
        public void VDCRealPrecision_Write_Binary()
        {
            TestCommand(cgm => new VDCRealPrecision(cgm, Precision.Fixed_32), cmd => cmd.Value == Precision.Fixed_32);
            TestCommand(cgm => new VDCRealPrecision(cgm, Precision.Fixed_64), cmd => cmd.Value == Precision.Fixed_64);
            TestCommand(cgm => new VDCRealPrecision(cgm, Precision.Floating_32), cmd => cmd.Value == Precision.Floating_32);
            TestCommand(cgm => new VDCRealPrecision(cgm, Precision.Floating_64), cmd => cmd.Value == Precision.Floating_64);
        }

        [Test]
        public void VDCType_Write_Binary()
        {
            TestCommand(cgm => new VDCType(cgm, VDCType.Type.Integer), cmd => cmd.Value == VDCType.Type.Integer);
            TestCommand(cgm => new VDCType(cgm, VDCType.Type.Real), cmd => cmd.Value == VDCType.Type.Real);
        }

        [TestCase(Precision.Fixed_32, 5, 5)]
        [TestCase(Precision.Fixed_32, 10.2, 10.199996948242188)]
        [TestCase(Precision.Fixed_64, 5, 5)]
        [TestCase(Precision.Fixed_64, 10.2, 10.199999999953434)]
        [TestCase(Precision.Floating_32, 5, 5)]
        [TestCase(Precision.Floating_32, 10.2, 10.2)]
        [TestCase(Precision.Floating_64, 5, 5)]
        [TestCase(Precision.Floating_64, 10.2, 10.2)]
        public void WriteReal_RealPrecision_Write_Binary(Precision precision, double value, double expected)
        {
            TestCommand(cgm =>
            {
                cgm.Commands.Add(new RealPrecision(cgm, precision));
                cgm.RealPrecision = precision;
                return new CharacterExpansionFactor(cgm, value);
            }, cmd => cmd.Factor == expected);
        }

        private void TestCommand<TCommand>(Func<CGMFile, TCommand> commandCreator, Func<TCommand, bool> check) where TCommand : Command
        {
            var cgm = new BinaryCGMFile();
            cgm.Commands.Add(commandCreator(cgm));

            var content = cgm.GetContent();
            var binaryFile = new BinaryCGMFile(new MemoryStream(content));

            var newcommand = binaryFile.Commands.FirstOrDefault(cmd => cmd is TCommand) as TCommand;

            if (newcommand == null)
                Assert.Fail($"Parsed CGM did not contain {typeof(TCommand)} command!");

            var allMessages = string.Join(Environment.NewLine, binaryFile.Messages.Select(m => m.ToString().ToArray()));

            Assert.IsTrue(check(newcommand), allMessages);
        }

        private void TestCommand<TCommand>(Func<CGMFile, TCommand> commandCreator, Action<TCommand> assertLogic) where TCommand : Command
        {
            TestCommand<TCommand>(commandCreator, cmd => { assertLogic(cmd); return true; });
        }

        private bool IsColorIndex(CGMColor color)
        {
            return color.ColorIndex == Color_Index.ColorIndex;
        }

        private bool IsColorIndex2(CGMColor color)
        {
            return color.ColorIndex == Color_Index2.ColorIndex;
        }

    }
}
