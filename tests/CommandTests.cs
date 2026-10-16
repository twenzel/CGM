using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using codessentials.CGM.Classes;
using codessentials.CGM.Commands;
using codessentials.CGM.Elements;
using NUnit.Framework;
using Shouldly;

namespace codessentials.CGM.Tests
{
    [TestFixture]
    public class CommandTests
    {
        private static readonly CgmColor Color_Index = new CgmColor() { ColorIndex = 2 };
        private static readonly CgmColor Color_Index2 = new CgmColor() { ColorIndex = 3 };
        private static readonly CgmColor Color_Color = new CgmColor() { Color = System.Drawing.Color.Red };
        private static readonly CgmColor Color_Color2 = new CgmColor() { Color = System.Drawing.Color.Peru };
        private static readonly CgmPoint Point = new CgmPoint(2, 2);
        private static readonly CgmPoint Point2 = new CgmPoint(5, 8);
        private static readonly CgmPoint Point3 = new CgmPoint(4, 99);

        [Test]
        public void AlternateCharacterSetIndex_Write_Binary()
        {
            TestCommand(cgm => new AlternateCharacterSetIndex(cgm, 2), cmd => cmd.Index == 2);
        }

        [Test]
        public void AppendText_Write_Binary()
        {
            TestCommand(cgm => new AppendText(cgm, AppendText.FinalType.FINAL, "test"), cmd => cmd.Final == AppendText.FinalType.FINAL && cmd.Text == "test");
            TestCommand(cgm => new AppendText(cgm, AppendText.FinalType.NOTFINAL, "test2"), cmd => cmd.Final == AppendText.FinalType.NOTFINAL && cmd.Text == "test2");
        }

        [Test]
        public void ApplicationData_Write_Binary()
        {
            TestCommand(cgm => new ApplicationData(cgm, 2, "test"), cmd => cmd.Identifier == 2 && cmd.Data == "test");
        }

        [Test]
        public void ApplicationStructureAttribute_Write_Binary()
        {
            var sdr = new StructuredDataRecord();
            sdr.Add(StructuredDataRecord.StructuredDataType.S, new object[] { "lala" });

            TestCommand(cgm => new ApplicationStructureAttribute(cgm, "test", sdr), cmd =>
            {
                cmd.AttributeType.ShouldBe("test");
                cmd.Data.Members.ShouldHaveCount(1);
                cmd.Data.Members[0].Type.ShouldBe(StructuredDataRecord.StructuredDataType.S);
                cmd.Data.Members[0].Data[0].ShouldBe("lala");
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
            var info = new AspectSourceFlags.AspectSourceFlagsInfo() { Type = AspectSourceFlags.ASFType.edgecolour, Value = AspectSourceFlags.ASFValue.BUNDLED };
            var info2 = new AspectSourceFlags.AspectSourceFlagsInfo() { Type = AspectSourceFlags.ASFType.hatchindex, Value = AspectSourceFlags.ASFValue.INDIV };
            var info3 = new AspectSourceFlags.AspectSourceFlagsInfo() { Type = AspectSourceFlags.ASFType.textcolour, Value = AspectSourceFlags.ASFValue.BUNDLED };

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
            }, cmd => cmd.Color.Color.ToArgb().ShouldBe(Color_Color.Color.ToArgb()));

            TestCommand(cgm =>
            {
                cgm.Commands.Add(new ColourSelectionMode(cgm, ColourSelectionMode.Type.DIRECT));
                cgm.ColourSelectionMode = ColourSelectionMode.Type.DIRECT;
                return new AuxiliaryColour(cgm, Color_Color2);
            }, cmd => cmd.Color.Color.ToArgb().ShouldBe(Color_Color2.Color.ToArgb()));
        }

        [Test]
        public void BackgroundColour_Write_Binary()
        {
            TestCommand(cgm => new BackgroundColour(cgm, System.Drawing.Color.Red), cmd => cmd.Color.ToArgb().ShouldBe(System.Drawing.Color.Red.ToArgb()));
            TestCommand(cgm => new BackgroundColour(cgm, System.Drawing.Color.Purple), cmd => cmd.Color.ToArgb().ShouldBe(System.Drawing.Color.Purple.ToArgb()));
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
            var position = new CgmPoint(55.879, 1.654889);
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

            BeginTileArray TileArrayFunc(CgmFile cgm)
            {
                return new BeginTileArray(cgm, position, cellPathDirection, lineProgressionDirection, nTilesInPathDirection, nTilesInLineDirection,
                    nCellsPerTileInPathDirection, nCellsPerTileInLineDirection, cellSizeInPathDirection, cellSizeInLineDirection, imageOffsetInPathDirection,
                    imageOffsetInLineDirection, nCellsInPathDirection, nCellsInLineDirection);
            }

            bool CheckFunc(BeginTileArray cmd)
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
            }

            TestCommand(cgm =>
            {
                cgm.Commands.Add(new VdcType(cgm, VdcType.Type.Real));
                cgm.Commands.Add(new VdcRealPrecision(cgm, Precision.Floating_32));
                cgm.Commands.Add(new RealPrecision(cgm, Precision.Floating_32));
                cgm.VDCType = VdcType.Type.Real;
                cgm.VDCRealPrecision = Precision.Floating_32;
                cgm.RealPrecision = Precision.Floating_32;

                return TileArrayFunc(cgm);
            }, CheckFunc);
        }

        [Test]
        public void BitonalTile_Write_Binary()
        {
            var color1 = new CgmColor() { ColorIndex = 5 };
            var color2 = new CgmColor() { ColorIndex = 4 };

            var sdr = new StructuredDataRecord();
            sdr.Add(StructuredDataRecord.StructuredDataType.E, new object[] { 2 });
            var image = new MemoryStream(new byte[] { 1, 20, 30, 5, 45 });

            TestCommand(cgm => new BitonalTile(cgm, CompressionType.BITMAP, 1, color1, color2, sdr, image), cmd =>
            {
                cmd.CompressionType.ShouldBe(CompressionType.BITMAP);
                cmd.RowPaddingIndicator.ShouldBe(1);
                cmd.Backgroundcolor.ShouldBe(color1);
                cmd.Foregroundcolor.ShouldBe(color2);
            });

            TestCommand(cgm => new BitonalTile(cgm, CompressionType.PNG, 88, color1, color2, sdr, image), cmd =>
            {
                cmd.CompressionType.ShouldBe(CompressionType.PNG);
                cmd.RowPaddingIndicator.ShouldBe(88);
                cmd.Backgroundcolor.ShouldBe(color1);
                cmd.Foregroundcolor.ShouldBe(color2);
                cmd.DataRecord.Members.ShouldHaveCount(1);
                cmd.DataRecord.Members[0].Type.ShouldBe(StructuredDataRecord.StructuredDataType.E);
                cmd.Image.ToArray().ShouldBeEquivalentTo(image.ToArray());
            });
        }

        [Test]
        public void CellArray_Write_Binary()
        {
            var point1 = new CgmPoint(1, 1);
            var point2 = new CgmPoint(2, 2);

            TestCommand(cgm => new CellArray(cgm, 0, 1, 2, point1, point2, point2, 0, new[] { Color_Index, Color_Index2 }), cmd =>
            {
                cmd.RepresentationFlag.ShouldBe(0);
                cmd.Nx.ShouldBe(1);
                cmd.Ny.ShouldBe(2);
                cmd.P.ShouldBe(point1);
                cmd.Q.ShouldBe(point2);
                cmd.R.ShouldBe(point2);
                cmd.LocalColorPrecision.ShouldBe(0);
                cmd.Colors[0].ShouldBe(Color_Index);
                cmd.Colors[1].ShouldBe(Color_Index2);
            });

            TestCommand(cgm => new CellArray(cgm, 1, 1, 2, point1, point2, point2, 8, new[] { Color_Index, Color_Index2 }), cmd =>
            {
                cmd.RepresentationFlag.ShouldBe(1);
                cmd.Nx.ShouldBe(1);
                cmd.Ny.ShouldBe(2);
                cmd.P.ShouldBe(point1);
                cmd.Q.ShouldBe(point2);
                cmd.R.ShouldBe(point2);
                cmd.LocalColorPrecision.ShouldBe(8);
                cmd.Colors[0].ShouldBe(Color_Index);
                cmd.Colors[1].ShouldBe(Color_Index2);
            });
        }

        [Test]
        public void CharacterCodingAnnouncer_Write_Binary()
        {
            TestCommand(cgm => new CharacterCodingAnnouncer(cgm, CharacterCodingAnnouncer.Type.BASIC_7_BIT), cmd => cmd.Value.ShouldBe(CharacterCodingAnnouncer.Type.BASIC_7_BIT));
            TestCommand(cgm => new CharacterCodingAnnouncer(cgm, CharacterCodingAnnouncer.Type.BASIC_8_BIT), cmd => cmd.Value.ShouldBe(CharacterCodingAnnouncer.Type.BASIC_8_BIT));
            TestCommand(cgm => new CharacterCodingAnnouncer(cgm, CharacterCodingAnnouncer.Type.EXTENDED_8_BIT), cmd => cmd.Value.ShouldBe(CharacterCodingAnnouncer.Type.EXTENDED_8_BIT));
        }

        [Test]
        public void CharacterExpansionFactore_Write_Binary()
        {
            TestCommand(cgm => new CharacterExpansionFactor(cgm, 12.2), cmd => cmd.Factor.ShouldBe(12.199996948242188));
            TestCommand(cgm => new CharacterExpansionFactor(cgm, 5), cmd => cmd.Factor.ShouldBe(5));
            TestCommand(cgm => new CharacterExpansionFactor(cgm, 45.689), cmd => cmd.Factor.ShouldBe(45.688995361328125));
        }

        [Test]
        public void CharacterHeight_Write_Binary()
        {
            TestCommand(cgm =>
            {
                cgm.Commands.Add(new VdcRealPrecision(cgm, Precision.Fixed_32));
                cgm.VDCRealPrecision = Precision.Fixed_32;
                cgm.Commands.Add(new VdcType(cgm, VdcType.Type.Real));
                cgm.VDCType = VdcType.Type.Real;
                return new CharacterHeight(cgm, 12.2);
            }, cmd => cmd.Height.ShouldBe(12.199996948242188));

            TestCommand(cgm => new CharacterHeight(cgm, 5), cmd => cmd.Height.ShouldBe(5));
        }

        [Test]
        public void CharacterOrientationt_Write_Binary()
        {
            TestCommand(cgm =>
            {
                cgm.Commands.Add(new VdcRealPrecision(cgm, Precision.Fixed_32));
                cgm.VDCRealPrecision = Precision.Fixed_32;
                cgm.Commands.Add(new VdcType(cgm, VdcType.Type.Real));
                cgm.VDCType = VdcType.Type.Real;
                return new CharacterOrientation(cgm, 12.2, 1, 5.5, 4);
            }, cmd =>
            {
                cmd.Xup.ShouldBe(12.199996948242188);
                cmd.yup.ShouldBe(1);
                cmd.Xbase.ShouldBe(5.5);
                cmd.Ybase.ShouldBe(4);
            });

            TestCommand(cgm => new CharacterOrientation(cgm, 5, 3, 2, 1), cmd =>
            {
                cmd.Xup.ShouldBe(5);
                cmd.yup.ShouldBe(3);
                cmd.Xbase.ShouldBe(2);
                cmd.Ybase.ShouldBe(1);
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
            TestCommand(cgm => new CharacterSetIndex(cgm, 1), cmd => cmd.Index.ShouldBe(1));
            TestCommand(cgm => new CharacterSetIndex(cgm, 0), cmd => cmd.Index.ShouldBe(0));
        }

        [Test]
        public void CharacterSpacing_Write_Binary()
        {
            TestCommand(cgm => new CharacterSpacing(cgm, 2), cmd => cmd.Space.ShouldBe(2));
        }

        [Test]
        public void CircleElement_Write_Binary()
        {
            var point = new CgmPoint(2, 2);

            TestCommand(cgm => new CircleElement(cgm, point, 2), cmd =>
            {
                cmd.Center.ShouldBe(point);
                cmd.Radius.ShouldBe(2);
            });
        }

        [Test]
        public void CircularArc3Point_Write_Binary()
        {
            var point = new CgmPoint(2, 2);
            var point2 = new CgmPoint(5, 2);
            var point3 = new CgmPoint(4, 4);

            TestCommand(cgm => new CircularArc3Point(cgm, point, point2, point3), cmd =>
            {
                cmd.P1.ShouldBe(point);
                cmd.P2.ShouldBe(point2);
                cmd.P3.ShouldBe(point3);
            });
        }

        [Test]
        public void CircularArc3PointClose_Write_Binary()
        {
            var point = new CgmPoint(2, 2);
            var point2 = new CgmPoint(5, 2);
            var point3 = new CgmPoint(4, 4);

            TestCommand(cgm => new CircularArc3PointClose(cgm, point, point2, point3, ClosureType.CHORD), cmd =>
            {
                cmd.P1.ShouldBe(point);
                cmd.P2.ShouldBe(point2);
                cmd.P3.ShouldBe(point3);
                cmd.Type.ShouldBe(ClosureType.CHORD);
            });
        }

        [Test]
        public void CircularArcCentre_Write_Binary()
        {
            var point = new CgmPoint(2, 2);

            TestCommand(cgm => new CircularArcCentre(cgm, point, 1, 2, 3, 4, 5), cmd =>
            {
                cmd.Center.ShouldBe(point);
                cmd.StartDeltaX.ShouldBe(1);
                cmd.StartDeltaY.ShouldBe(2);
                cmd.EndDeltaX.ShouldBe(3);
                cmd.EndDeltaY.ShouldBe(4);
                cmd.Radius.ShouldBe(5);
            });
        }

        [Test]
        public void CircularArcCentreClose_Write_Binary()
        {
            var point = new CgmPoint(2, 2);

            TestCommand(cgm => new CircularArcCentreClose(cgm, point, 1, 2, 3, 4, 5, ClosureType.PIE), cmd =>
            {
                cmd.Center.ShouldBe(point);
                cmd.StartDeltaX.ShouldBe(1);
                cmd.StartDeltaY.ShouldBe(2);
                cmd.EndDeltaX.ShouldBe(3);
                cmd.EndDeltaY.ShouldBe(4);
                cmd.Radius.ShouldBe(5);
                cmd.Type.ShouldBe(ClosureType.PIE);
            });
        }

        [Test]
        public void CircularArcCentreReversed_Write_Binary()
        {
            var point = new CgmPoint(2, 2);

            TestCommand(cgm => new CircularArcCentreReversed(cgm, point, 1, 2, 3, 4, 5), cmd =>
            {
                cmd.Center.ShouldBe(point);
                cmd.StartDeltaX.ShouldBe(1);
                cmd.StartDeltaY.ShouldBe(2);
                cmd.EndDeltaX.ShouldBe(3);
                cmd.EndDeltaY.ShouldBe(4);
                cmd.Radius.ShouldBe(5);
            });
        }

        [Test]
        public void ClipIndicator_Write_Binary()
        {
            TestCommand(cgm => new ClipIndicator(cgm, true), cmd => cmd.Flag.ShouldBeTrue());
            TestCommand(cgm => new ClipIndicator(cgm, false), cmd => cmd.Flag.ShouldBeFalse());
        }

        [Test]
        public void ClipInheritance_Write_Binary()
        {
            TestCommand(cgm => new ClipInheritance(cgm, ClipInheritance.Value.INTERSECTION), cmd => cmd.Data.ShouldBe(ClipInheritance.Value.INTERSECTION));
            TestCommand(cgm => new ClipInheritance(cgm, ClipInheritance.Value.STLIST), cmd => cmd.Data.ShouldBe(ClipInheritance.Value.STLIST));
        }

        [Test]
        public void ClipRectangle_Write_Binary()
        {
            var point = new CgmPoint(2, 2);
            var point2 = new CgmPoint(5, 3);

            TestCommand(cgm => new ClipRectangle(cgm, point, point2), cmd =>
            {
                cmd.Point1.ShouldBe(point);
                cmd.Point2.ShouldBe(point2);
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
                cmd.CalibrationSelection.ShouldBe(2);
                cmd.ReferenceX.ShouldBe(2.1999969482421875);
                cmd.ReferenceY.ShouldBe(2.1999969482421875);
                cmd.ReferenceZ.ShouldBe(2.1999969482421875);
                cmd.Xr.ShouldBe(2.1999969482421875);
                cmd.Xg.ShouldBe(2.1999969482421875);
                cmd.Xb.ShouldBe(2.1999969482421875);
                cmd.Yr.ShouldBe(2.1999969482421875);
                cmd.Yg.ShouldBe(2.1999969482421875);
                cmd.Yb.ShouldBe(2.1999969482421875);
                cmd.Zr.ShouldBe(2.1999969482421875);
                cmd.Zg.ShouldBe(2.1999969482421875);
                cmd.Zb.ShouldBe(2.1999969482421875);
                cmd.Ra.ShouldBe(2.1999969482421875);
                cmd.Rb.ShouldBe(2.1999969482421875);
                cmd.Rc.ShouldBe(2.1999969482421875);
                cmd.Ga.ShouldBe(2.1999969482421875);
                cmd.Gb.ShouldBe(2.1999969482421875);
                cmd.Gc.ShouldBe(2.1999969482421875);
                cmd.Ba.ShouldBe(2.1999969482421875);
                cmd.Bb.ShouldBe(2.1999969482421875);
                cmd.Bc.ShouldBe(2.1999969482421875);
                cmd.TableEntries.ShouldBe(1);
                cmd.LookupR.ShouldHaveCount(1);
                cmd.LookupR[0].ShouldBe(new Tuple<double, double>(5, 4));
                cmd.LookupG.ShouldHaveCount(1);
                cmd.LookupB.ShouldHaveCount(1);
                cmd.NumberOfGridLocations.ShouldBe(2);
                cmd.CmykGridLocations.ShouldHaveCount(2);
                cmd.XyzGridLocations.ShouldHaveCount(2);
            });
        }


        [Test]
        public void ColourIndexPrecision_Write_Binary()
        {
            TestCommand(cgm => new ColourIndexPrecision(cgm, 8), cmd => cmd.Precision.ShouldBe(8));
        }

        [Test]
        public void ColourModel_Write_Binary()
        {
            TestCommand(cgm => new ColourModel(cgm, ColourModel.Model.RGB), cmd => cmd.Value.ShouldBe(ColourModel.Model.RGB));
            TestCommand(cgm => new ColourModel(cgm, ColourModel.Model.CMYK), cmd => cmd.Value.ShouldBe(ColourModel.Model.CMYK));
        }

        [Test]
        public void ColourPrecision_Write_Binary()
        {
            TestCommand(cgm => new ColourPrecision(cgm, 8), cmd => cmd.Precision.ShouldBe(8));
        }


        [Test]
        public void ColourSelectionMode_Write_Binary()
        {
            TestCommand(cgm => new ColourSelectionMode(cgm, ColourSelectionMode.Type.DIRECT), cmd => cmd.Mode.ShouldBe(ColourSelectionMode.Type.DIRECT));
            TestCommand(cgm => new ColourSelectionMode(cgm, ColourSelectionMode.Type.INDEXED), cmd => cmd.Mode.ShouldBe(ColourSelectionMode.Type.INDEXED));
        }

        [Test]
        public void ColourTable_Write_Binary()
        {
            TestCommand(cgm => new ColourTable(cgm, 1, new[] { Color.Red, Color.Plum }), cmd =>
            {
                cmd.StartIndex.ShouldBe(1);
                cmd.Colors.ShouldHaveCount(2);
                cmd.Colors[0].ToArgb().ShouldBe(Color.Red.ToArgb());
                cmd.Colors[1].ToArgb().ShouldBe(Color.Plum.ToArgb());
            });
            TestCommand(cgm => new ColourTable(cgm, 5, new Color[] { }), cmd =>
            {
                cmd.StartIndex.ShouldBe(5);
                cmd.Colors.ShouldHaveCount(0);
            });
        }

        [Test]
        public void ColourValueExtent_Write_Binary()
        {
            TestCommand(cgm => new ColourValueExtent(cgm, new[] { 0, 0, 0 }, new[] { 255, 255, 255 }, 2, 0, 0), cmd =>
            {
                cmd.MinimumColorValueRGB.ShouldHaveCount(3);
                cmd.MinimumColorValueRGB[0].ShouldBe(0);
                cmd.MinimumColorValueRGB[1].ShouldBe(0);
                cmd.MinimumColorValueRGB[2].ShouldBe(0);
                cmd.MaximumColorValueRGB.ShouldHaveCount(3);
                cmd.MaximumColorValueRGB[0].ShouldBe(255);
                cmd.MaximumColorValueRGB[1].ShouldBe(255);
                cmd.MaximumColorValueRGB[2].ShouldBe(255);
                cmd.FirstComponentScale.ShouldBe(0);
                cmd.SecondComponentScale.ShouldBe(0);
                cmd.ThirdComponentScale.ShouldBe(0);
            });

            TestCommand(cgm => new ColourValueExtent(cgm, new[] { 10, 20, 30 }, new[] { 200, 200, 200 }, 0, 0, 0), cmd =>
            {
                cmd.MinimumColorValueRGB.ShouldHaveCount(3);
                cmd.MinimumColorValueRGB[0].ShouldBe(10);
                cmd.MinimumColorValueRGB[1].ShouldBe(20);
                cmd.MinimumColorValueRGB[2].ShouldBe(30);
                cmd.MaximumColorValueRGB.ShouldHaveCount(3);
                cmd.MaximumColorValueRGB[0].ShouldBe(200);
                cmd.MaximumColorValueRGB[1].ShouldBe(200);
                cmd.MaximumColorValueRGB[2].ShouldBe(200);
                cmd.FirstComponentScale.ShouldBe(0);
                cmd.SecondComponentScale.ShouldBe(0);
                cmd.ThirdComponentScale.ShouldBe(0);
            });

            TestCommand(cgm =>
            {
                cgm.ColourModel = ColourModel.Model.RGB_RELATED;
                cgm.Commands.Add(new ColourModel(cgm, ColourModel.Model.RGB_RELATED));
                return new ColourValueExtent(cgm, new[] { 10, 20, 30 }, new[] { 200, 200, 200 }, 1, 2, 55);
            }, cmd =>
            {
                cmd.MinimumColorValueRGB.ShouldHaveCount(0);
                cmd.MaximumColorValueRGB.ShouldHaveCount(0);
                cmd.FirstComponentScale.ShouldBe(1);
                cmd.SecondComponentScale.ShouldBe(2);
                cmd.ThirdComponentScale.ShouldBe(55);
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
                cmd.Id.ShouldBe(2);
                cmd.XScale.ShouldBe(4);
                cmd.XRotation.ShouldBe(0);
                cmd.YRotation.ShouldBe(8);
                cmd.YScale.ShouldBe(6);
                cmd.XTranslation.ShouldBe(2);
                cmd.YTranslation.ShouldBe(1);
                cmd.Flag.ShouldBe(true);
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
                cmd.FirstCorner.FirstPoint.ValueInt.ShouldBe(6);
                cmd.FirstCorner.SecondPoint.ValueInt.ShouldBe(3);
                cmd.SecondCorner.FirstPoint.ValueInt.ShouldBe(8);
                cmd.SecondCorner.SecondPoint.ValueInt.ShouldBe(1);
            });

            corner1 = new ViewportPoint() { FirstPoint = new VC() { ValueReal = 6 }, SecondPoint = new VC() { ValueReal = 3 } };
            corner2 = new ViewportPoint() { FirstPoint = new VC() { ValueReal = 8 }, SecondPoint = new VC() { ValueReal = 1 } };

            TestCommand(cgm => new DeviceViewport(cgm, corner1, corner2), cmd =>
            {
                cmd.FirstCorner.FirstPoint.ValueReal.ShouldBe(6);
                cmd.FirstCorner.SecondPoint.ValueReal.ShouldBe(3);
                cmd.SecondCorner.FirstPoint.ValueReal.ShouldBe(8);
                cmd.SecondCorner.SecondPoint.ValueReal.ShouldBe(1);
            });
        }

        [Test]
        public void DeviceViewportMapping_Write_Binary()
        {
            TestCommand(cgm => new DeviceViewportMapping(cgm, DeviceViewportMapping.Isotropy.FORCED, DeviceViewportMapping.Horizontalalignment.CTR, DeviceViewportMapping.Verticalalignment.CTR), cmd =>
            {
                cmd.IsotropyValue.ShouldBe(DeviceViewportMapping.Isotropy.FORCED);
                cmd.HorizontalAlignment.ShouldBe(DeviceViewportMapping.Horizontalalignment.CTR);
                cmd.VerticalAlignment.ShouldBe(DeviceViewportMapping.Verticalalignment.CTR);
            });

            TestCommand(cgm => new DeviceViewportMapping(cgm, DeviceViewportMapping.Isotropy.NOTFORCED, DeviceViewportMapping.Horizontalalignment.LEFT, DeviceViewportMapping.Verticalalignment.BOTTOM), cmd =>
            {
                cmd.IsotropyValue.ShouldBe(DeviceViewportMapping.Isotropy.NOTFORCED);
                cmd.HorizontalAlignment.ShouldBe(DeviceViewportMapping.Horizontalalignment.LEFT);
                cmd.VerticalAlignment.ShouldBe(DeviceViewportMapping.Verticalalignment.BOTTOM);
            });

        }

        [Test]
        public void DeviceViewportSpecificationMode_Write_Binary()
        {
            TestCommand(cgm => new DeviceViewportSpecificationMode(cgm, DeviceViewportSpecificationMode.Mode.FRACTION, 1), cmd => { cmd.Value.ShouldBe(DeviceViewportSpecificationMode.Mode.FRACTION); cmd.MetricScaleFactor.ShouldBe(1); });
            TestCommand(cgm => new DeviceViewportSpecificationMode(cgm, DeviceViewportSpecificationMode.Mode.MM, 2), cmd => { cmd.Value.ShouldBe(DeviceViewportSpecificationMode.Mode.MM); cmd.MetricScaleFactor.ShouldBe(2); });
            TestCommand(cgm => new DeviceViewportSpecificationMode(cgm, DeviceViewportSpecificationMode.Mode.PHYDEVCOORD, 5), cmd => { cmd.Value.ShouldBe(DeviceViewportSpecificationMode.Mode.PHYDEVCOORD); cmd.MetricScaleFactor.ShouldBe(5); });
        }


        [Test]
        public void DisjointPolyline_Write_Binary()
        {
            TestCommand(cgm => new DisjointPolyline(cgm, new[] { new KeyValuePair<CgmPoint, CgmPoint>(new CgmPoint(1, 2), new CgmPoint(5, 6)) }), cmd =>
            {
                cmd.Lines.ShouldHaveCount(1);
                cmd.Lines[0].Key.X.ShouldBe(1);
                cmd.Lines[0].Key.Y.ShouldBe(2);
                cmd.Lines[0].Value.X.ShouldBe(5);
                cmd.Lines[0].Value.Y.ShouldBe(6);
            });
        }


        [Test]
        public void EdgeBundleIndex_Write_Binary()
        {
            TestCommand(cgm => new EdgeBundleIndex(cgm, 8), cmd => cmd.Index.ShouldBe(8));
        }

        [Test]
        public void EdgeCap_Write_Binary()
        {
            TestCommand(cgm => new EdgeCap(cgm, LineCapIndicator.BUTT, DashCapIndicator.MATCH), cmd =>
            {
                cmd.LineIndicator.ShouldBe(LineCapIndicator.BUTT);
                cmd.DashIndicator.ShouldBe(DashCapIndicator.MATCH);
            });

            TestCommand(cgm => new EdgeCap(cgm, LineCapIndicator.ROUND, DashCapIndicator.UNSPECIFIED), cmd =>
            {
                cmd.LineIndicator.ShouldBe(LineCapIndicator.ROUND);
                cmd.DashIndicator.ShouldBe(DashCapIndicator.UNSPECIFIED);
            });
        }

        [Test]
        public void EdgeClipping_Write_Binary()
        {
            TestCommand(cgm => new EdgeClipping(cgm, ClippingMode.LOCUSTHENSHAPE), cmd => cmd.Mode.ShouldBe(ClippingMode.LOCUSTHENSHAPE));
        }


        [Test]
        public void EdgeColour_Write_Binary()
        {
            TestCommand(cgm => new EdgeColour(cgm, Color_Index), cmd => IsColorIndex(cmd.Color));
        }

        [Test]
        public void EdgeJoin_Write_Binary()
        {
            TestCommand(cgm => new EdgeJoin(cgm, JoinIndicator.BEVEL), cmd => cmd.Type.ShouldBe(JoinIndicator.BEVEL));
        }

        [Test]
        public void EdgeRepresentation_Write_Binary()
        {
            TestCommand(cgm => new EdgeRepresentation(cgm, 2, 3, 5, Color_Index), cmd =>
            {
                cmd.BundleIndex.ShouldBe(2);
                cmd.EdgeType.ShouldBe(3);
                cmd.EdgeWidth.ShouldBe(5);
                cmd.EdgeColor.ShouldBe(Color_Index);
            });
        }

        [Test]
        public void EdgeType_Write_Binary()
        {
            TestCommand(cgm => new EdgeType(cgm, DashType.DASH), cmd => cmd.Type.ShouldBe(DashType.DASH));
        }

        [Test]
        public void EdgeTypeContinuation_Write_Binary()
        {
            TestCommand(cgm => new EdgeTypeContinuation(cgm, 5), cmd => cmd.Mode.ShouldBe(5));
        }

        [Test]
        public void EdgeTypeInitialOffset_Write_Binary()
        {
            TestCommand(cgm => new EdgeTypeInitialOffset(cgm, 5), cmd => cmd.Offset.ShouldBe(5));
        }

        [Test]
        public void EdgeVisibility_Write_Binary()
        {
            TestCommand(cgm => new EdgeVisibility(cgm, true), cmd => cmd.IsVisible.ShouldBe(true));
            TestCommand(cgm => new EdgeVisibility(cgm, false), cmd => cmd.IsVisible.ShouldBe(false));
        }

        [Test]
        public void EdgeWidth_Write_Binary()
        {
            TestCommand(cgm => new EdgeWidth(cgm, 5), cmd => cmd.Width.ShouldBe(5));
        }

        [Test]
        public void EdgeWidthSpecificationMode_Write_Binary()
        {
            TestCommand(cgm => new EdgeWidthSpecificationMode(cgm, SpecificationMode.FRACTIONAL), cmd => cmd.Mode.ShouldBe(SpecificationMode.FRACTIONAL));
        }

        [Test]
        public void EllipseElement_Write_Binary()
        {
            TestCommand(cgm => new EllipseElement(cgm, Point, Point, Point2), cmd =>
            {
                cmd.Center.ShouldBe(Point);
                cmd.FirstConjugateDiameterEndPoint.ShouldBe(Point);
                cmd.SecondConjugateDiameterEndPoint.ShouldBe(Point2);
            });
        }

        [Test]
        public void EllipticalArc_Write_Binary()
        {
            TestCommand(cgm => new EllipticalArc(cgm, 5, 2, 4, 7, Point, Point, Point2), cmd =>
            {
                cmd.Center.ShouldBe(Point);
                cmd.FirstConjugateDiameterEndPoint.ShouldBe(Point);
                cmd.SecondConjugateDiameterEndPoint.ShouldBe(Point2);
                cmd.StartVectorDeltaX.ShouldBe(5);
                cmd.StartVectorDeltaY.ShouldBe(2);
                cmd.EndVectorDeltaX.ShouldBe(4);
                cmd.EndVectorDeltaY.ShouldBe(7);
            });
        }

        [Test]
        public void EllipticalArcClosec_Write_Binary()
        {
            TestCommand(cgm => new EllipticalArcClose(cgm, ClosureType.PIE, 5, 2, 4, 7, Point, Point, Point2), cmd =>
            {
                cmd.ClosureType.ShouldBe(ClosureType.PIE);
                cmd.Center.ShouldBe(Point);
                cmd.FirstConjugateDiameterEndPoint.ShouldBe(Point);
                cmd.SecondConjugateDiameterEndPoint.ShouldBe(Point2);
                cmd.StartVectorDeltaX.ShouldBe(5);
                cmd.StartVectorDeltaY.ShouldBe(2);
                cmd.EndVectorDeltaX.ShouldBe(4);
                cmd.EndVectorDeltaY.ShouldBe(7);
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
                cmd.Identifier.ShouldBe(3);
                cmd.DataRecord.ShouldBe("test1");
            });
        }

        [Test]
        public void FillBundleIndex_Write_Binary()
        {
            TestCommand(cgm => new FillBundleIndex(cgm, 4), cmd => cmd.Index.ShouldBe(4));
        }

        [Test]
        public void FillColour_Write_Binary()
        {
            TestCommand(cgm => new FillColour(cgm, Color_Index), cmd => cmd.Color.ShouldBe(Color_Index));
        }

        [Test]
        public void FillReferencePoint_Write_Binary()
        {
            TestCommand(cgm => new FillReferencePoint(cgm, Point), cmd => cmd.Point.ShouldBe(Point));
        }

        [Test]
        public void FillRepresentation_Write_Binary()
        {
            TestCommand(cgm => new FillRepresentation(cgm, 3, InteriorStyle.Style.HATCH, Color_Index, 4, 2), cmd =>
            {
                cmd.BundleIndex.ShouldBe(3);
                cmd.Style.ShouldBe(InteriorStyle.Style.HATCH);
                cmd.Color.ShouldBe(Color_Index);
                cmd.HatchIndex.ShouldBe(4);
                cmd.PatternIndex.ShouldBe(2);
            });
        }

        [Test]
        public void FontList_Write_Binary()
        {
            TestCommand(cgm => new FontList(cgm, new[] { "Arial" }), cmd =>
            {
                cmd.FontNames.ShouldHaveCount(1);
                cmd.FontNames[0].ShouldBe("Arial");
            });

            TestCommand(cgm => new FontList(cgm, new[] { "Arial", "Arial Bold" }), cmd =>
            {
                cmd.FontNames.ShouldHaveCount(2);
                cmd.FontNames[0].ShouldBe("Arial");
                cmd.FontNames[1].ShouldBe("Arial Bold");
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
                cmd.Infos.ShouldHaveCount(1);
                cmd.Infos[0].Priority.ShouldBe(3);
                cmd.Infos[0].PropertyIndicator.ShouldBe(88);
                cmd.Infos[0].Value.Members.ShouldHaveCount(1);
                cmd.Infos[0].Value.Members[0].Type.ShouldBe(StructuredDataRecord.StructuredDataType.S);
                cmd.Infos[0].Value.Members[0].Data[0].ShouldBe("lala");
            });
        }

        [Test]
        public void GeneralizedDrawingPrimitive_Write_Binary()
        {
            TestCommand(cgm => new GeneralizedDrawingPrimitive(cgm, 3, new[] { Point, Point2 }, "test1"), cmd =>
            {
                cmd.Identifier.ShouldBe(3);
                cmd.Points.ShouldHaveCount(2);
                cmd.Points[0].ShouldBe(Point);
                cmd.DataRecord.ShouldBe("test1");
            });
        }


        [Test]
        public void GeneralizedTextPathMode_Write_Binary()
        {
            TestCommand(cgm => new GeneralizedTextPathMode(cgm, GeneralizedTextPathMode.TextPathMode.AXIS), cmd => cmd.Mode.ShouldBe(GeneralizedTextPathMode.TextPathMode.AXIS));
        }

        [Test]
        public void GeometricPatternDefinition_Write_Binary()
        {
            TestCommand(cgm => new GeometricPatternDefinition(cgm, 3, 5, Point, Point2), cmd =>
            {
                cmd.PatternIndex.ShouldBe(3);
                cmd.Identifier.ShouldBe(5);
                cmd.FirstCorner.ShouldBe(Point);
                cmd.SecondCorner.ShouldBe(Point2);
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
                cmd.CharacterSetIndex.ShouldBe(2);
                cmd.Type.ShouldBe(CharacterSetList.Type.COMPLETE_CODE);
                cmd.SequenceTail.ShouldBe("lala");
                cmd.OctetsPerCode.ShouldBe(4);
                cmd.GlyphSource.ShouldBe(22);
                cmd.CodeAssocs.Members.ShouldHaveCount(1);
                cmd.CodeAssocs.Members[0].Type.ShouldBe(StructuredDataRecord.StructuredDataType.S);
                cmd.CodeAssocs.Members[0].Data[0].ShouldBe("lala");
            });
        }

        [Test]
        public void HatchIndex_Write_Binary()
        {
            TestCommand(cgm => new HatchIndex(cgm, HatchIndex.HatchType.HORIZONTAL_VERTICAL_CROSSHATCH), cmd => cmd.Type.ShouldBe(HatchIndex.HatchType.HORIZONTAL_VERTICAL_CROSSHATCH));
        }

        [Test]
        public void HatchStyleDefinition_Write_Binary()
        {
            TestCommand(cgm => new HatchStyleDefinition(cgm, 3, HatchStyleDefinition.HatchStyle.CROSSHATCH, 1, 2, 3, 4, 5, new[] { 2, 3 }, new[] { 5, 5 }), cmd =>
            {
                cmd.Index.ShouldBe(3);
                cmd.Style.ShouldBe(HatchStyleDefinition.HatchStyle.CROSSHATCH);
                cmd.FirstDirX.ShouldBe(1);
                cmd.FirstDirY.ShouldBe(2);
                cmd.SecondDirX.ShouldBe(3);
                cmd.SecondDirY.ShouldBe(4);
                cmd.GapWidths.ShouldHaveCount(2);
                cmd.GapWidths[0].ShouldBe(2);
                cmd.GapWidths[1].ShouldBe(3);
                cmd.LineTypes.ShouldHaveCount(2);
                cmd.LineTypes[0].ShouldBe(5);
                cmd.LineTypes[1].ShouldBe(5);
            });
        }

        [Test]
        public void HyperbolicArc_Write_Binary()
        {
            TestCommand(cgm => new HyperbolicArc(cgm, Point, Point2, Point3, 2, 3, 4, 5), cmd =>
            {
                cmd.Center.ShouldBe(Point);
                cmd.TransverseRadius.ShouldBe(Point2);
                cmd.ConjugateRadius.ShouldBe(Point3);
                cmd.StartX.ShouldBe(2);
                cmd.StartY.ShouldBe(3);
                cmd.EndX.ShouldBe(4);
                cmd.EndY.ShouldBe(5);
            });
        }

        [Test]
        public void IndexPrecision_Write_Binary()
        {
            TestCommand(cgm => new IndexPrecision(cgm, 16), cmd => cmd.Precision.ShouldBe(16));
        }

        [Test]
        public void InheritanceFilter_Write_Binary()
        {
            TestCommand(cgm => new InheritanceFilter(cgm, new[] { InheritanceFilter.Filter.ALLFILL, InheritanceFilter.Filter.MARKERSIZE, InheritanceFilter.Filter.TEXTPATH }, 8), cmd =>
            {
                cmd.Values.ShouldHaveCount(3);
                cmd.Values[0].ShouldBe(InheritanceFilter.Filter.ALLFILL);
                cmd.Values[1].ShouldBe(InheritanceFilter.Filter.MARKERSIZE);
                cmd.Values[2].ShouldBe(InheritanceFilter.Filter.TEXTPATH);
                cmd.Setting.ShouldBe(8);
            });
        }

        [Test]
        public void IntegerPrecision_Write_Binary()
        {
            TestCommand(cgm => new IntegerPrecision(cgm, 16), cmd => cmd.Precision.ShouldBe(16));
        }

        [Test]
        public void InteriorStyle_Write_Binary()
        {
            TestCommand(cgm => new InteriorStyle(cgm, InteriorStyle.Style.HATCH), cmd => cmd.Value.ShouldBe(InteriorStyle.Style.HATCH));
        }

        [Test]
        public void InteriorStyleSpecificationMode_Write_Binary()
        {
            TestCommand(cgm => new InteriorStyleSpecificationMode(cgm, SpecificationMode.FRACTIONAL), cmd => cmd.Mode.ShouldBe(SpecificationMode.FRACTIONAL));
        }

        [Test]
        public void InterpolatedInterior_Write_Binary()
        {
            TestCommand(cgm => new InterpolatedInterior(cgm, 2, new[] { 2.0, 4 }, new[] { 7.0, 5 }, new[] { 44.0, 3 }, new[] { Color_Index, Color_Index2, Color_Index }), cmd =>
            {
                cmd.Style.ShouldBe(2);
                cmd.GeoX.ShouldHaveCount(2);
                cmd.GeoX[0].ShouldBe(2);
                cmd.GeoX[1].ShouldBe(4);
                cmd.GeoY.ShouldHaveCount(2);
                cmd.GeoY[0].ShouldBe(7);
                cmd.GeoY[1].ShouldBe(5);
                cmd.StageDesignators.ShouldHaveCount(2);
                cmd.StageDesignators[0].ShouldBe(44);
                cmd.StageDesignators[1].ShouldBe(3);
                cmd.Colors.ShouldHaveCount(3);
                cmd.Colors[0].ShouldBe(Color_Index);
                cmd.Colors[1].ShouldBe(Color_Index2);
                cmd.Colors[2].ShouldBe(Color_Index);
            });
        }

        [Test]
        public void LineAndEdgeTypeDefinition_Write_Binary()
        {
            TestCommand(cgm => new LineAndEdgeTypeDefinition(cgm, -2, 4, new[] { 5, 8, 3 }), cmd =>
            {
                cmd.LineType.ShouldBe(-2);
                cmd.DashCycleRepeatLength.ShouldBe(4);
                cmd.DashElements.ShouldHaveCount(3);
                cmd.DashElements[0].ShouldBe(5);
                cmd.DashElements[1].ShouldBe(8);
                cmd.DashElements[2].ShouldBe(3);
            });
        }

        [Test]
        public void LineBundleIndex_Write_Binary()
        {
            TestCommand(cgm => new LineBundleIndex(cgm, 3), cmd => cmd.Index.ShouldBe(3));
        }

        [Test]
        public void LineCap_Write_Binary()
        {
            TestCommand(cgm => new LineCap(cgm, LineCapIndicator.BUTT, DashCapIndicator.MATCH), cmd =>
            {
                cmd.LineIndicator.ShouldBe(LineCapIndicator.BUTT);
                cmd.DashIndicator.ShouldBe(DashCapIndicator.MATCH);
            });
        }

        [Test]
        public void LineClipping_Write_Binary()
        {
            TestCommand(cgm => new LineClipping(cgm, ClippingMode.LOCUS), cmd => cmd.Mode.ShouldBe(ClippingMode.LOCUS));
        }

        [Test]
        public void LineColour_Write_Binary()
        {
            TestCommand(cgm => new LineColour(cgm, Color_Index), cmd => cmd.Color.ShouldBe(Color_Index));
        }

        [Test]
        public void LineJoin_Write_Binary()
        {
            TestCommand(cgm => new LineJoin(cgm, JoinIndicator.BEVEL), cmd => cmd.Type.ShouldBe(JoinIndicator.BEVEL));
        }

        [Test]
        public void LineRepresentation_Write_Binary()
        {
            TestCommand(cgm => new LineRepresentation(cgm, 2, 5, 6, Color_Index), cmd =>
            {
                cmd.Index.ShouldBe(2);
                cmd.LineType.ShouldBe(5);
                cmd.LineWidth.ShouldBe(6);
                cmd.Color.ShouldBe(Color_Index);
            });
        }

        [Test]
        public void LineType_Write_Binary()
        {
            TestCommand(cgm => new LineType(cgm, DashType.DASH_DOT), cmd => cmd.Type.ShouldBe(DashType.DASH_DOT));
        }

        [Test]
        public void LineTypeContinuation_Write_Binary()
        {
            TestCommand(cgm => new LineTypeContinuation(cgm, 3), cmd => cmd.Mode.ShouldBe(3));
        }

        [Test]
        public void LineTypeInitialOffset_Write_Binary()
        {
            TestCommand(cgm => new LineTypeInitialOffset(cgm, 3), cmd => cmd.Offset.ShouldBe(3));
        }

        [Test]
        public void LineWidth_Write_Binary()
        {
            TestCommand(cgm => new LineWidth(cgm, 5), cmd => cmd.Width.ShouldBe(5));
        }

        [Test]
        public void LineWidthSpecificationMode_Write_Binary()
        {
            TestCommand(cgm => new LineWidthSpecificationMode(cgm, SpecificationMode.MM), cmd => cmd.Mode.ShouldBe(SpecificationMode.MM));
        }

        [Test]
        public void MarkerBundleIndex_Write_Binary()
        {
            TestCommand(cgm => new MarkerBundleIndex(cgm, 2), cmd => cmd.Index.ShouldBe(2));
        }

        [Test]
        public void MarkerClipping_Write_Binary()
        {
            TestCommand(cgm => new MarkerClipping(cgm, ClippingMode.LOCUSTHENSHAPE), cmd => cmd.Mode.ShouldBe(ClippingMode.LOCUSTHENSHAPE));
        }

        [Test]
        public void MarkerColour_Write_Binary()
        {
            TestCommand(cgm => new MarkerColour(cgm, Color_Index), cmd => cmd.Color.ShouldBe(Color_Index));
        }

        [Test]
        public void MarkerRepresentation_Write_Binary()
        {
            TestCommand(cgm => new MarkerRepresentation(cgm, 2, 5, 6, Color_Index), cmd =>
            {
                cmd.Index.ShouldBe(2);
                cmd.Type.ShouldBe(5);
                cmd.Size.ShouldBe(6);
                cmd.Color.ShouldBe(Color_Index);
            });
        }

        [Test]
        public void MarkerSize_Write_Binary()
        {
            TestCommand(cgm => new MarkerSize(cgm, 2), cmd => cmd.Width.ShouldBe(2));
        }

        [Test]
        public void MarkerSizeSpecificationMode_Write_Binary()
        {
            TestCommand(cgm => new MarkerSizeSpecificationMode(cgm, SpecificationMode.SCALED), cmd => cmd.Mode.ShouldBe(SpecificationMode.SCALED));
        }

        [Test]
        public void MarkerType_Write_Binary()
        {
            TestCommand(cgm => new MarkerType(cgm, MarkerType.Type.CIRCLE), cmd => cmd.Value.ShouldBe(MarkerType.Type.CIRCLE));
        }

        [Test]
        public void MaximumColourIndex_Write_Binary()
        {
            TestCommand(cgm => new MaximumColourIndex(cgm, 240), cmd => cmd.Value.ShouldBe(240));
        }

        [Test]
        public void MaximumVDCExtent_Write_Binary()
        {
            TestCommand(cgm => new MaximumVdcExtent(cgm, Point, Point2), cmd =>
            {
                cmd.FirstCorner.ShouldBe(Point);
                cmd.SecondCorner.ShouldBe(Point2);
            });
        }

        [Test]
        public void MessageCommand_Write_Binary()
        {
            TestCommand(cgm => new MessageCommand(cgm, MessageCommand.ActionType.Action, "testtt"), cmd =>
            {
                cmd.Action.ShouldBe(MessageCommand.ActionType.Action);
                cmd.Message.ShouldBe("testtt");
            });
        }

        [Test]
        public void MetafileDefaultsReplacement_Write_Binary()
        {
            TestCommand(cgm =>
            {
                var command1 = new MaximumColourIndex(cgm, 55);
                var command2 = new MaximumVdcExtent(cgm, Point, Point2);
                return new MetafileDefaultsReplacement(cgm, [command1, command2]);
            }, cmds =>
            {
                // MetafileDefaultReplacement command itself
                cmds.ShouldNotBeNull();
                cmds.ElementClass.ShouldBe(ClassCode.MetafileDescriptorElements);
                cmds.ElementId.ShouldBe((int)MetaFileDescriptorElement.METAFILE_DEFAULTS_REPLACEMENT);
                cmds.ShouldBeOfType<MetafileDefaultsReplacement>();

                cmds.EmbeddedCommands.Count.ShouldBeEquivalentTo(2);

                // First embedded command - MaximumColourIndex
                cmds.EmbeddedCommands[0].ShouldBeOfType<MaximumColourIndex>();
                (cmds.EmbeddedCommands[0] as MaximumColourIndex).Value.ShouldBe(55);

                // Second embedded command - MaximumVdcExtent
                cmds.EmbeddedCommands[1].ShouldBeOfType<MaximumVdcExtent>();
                (cmds.EmbeddedCommands[1] as MaximumVdcExtent).FirstCorner.ShouldBe(Point);
                (cmds.EmbeddedCommands[1] as MaximumVdcExtent).SecondCorner.ShouldBe(Point2);
            });
        }

        [Test]
        public void MetafileDescription_Write_Binary()
        {
            TestCommand(cgm => new MetafileDescription(cgm, "test"), cmd => cmd.Description.ShouldBe("test"));
            TestCommand(cgm => new MetafileDescription(cgm, "tes".PadRight(300)), cmd => cmd.Description.ShouldBe("tes".PadRight(300)));
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
            TestCommand(cgm => new MetafileVersion(cgm, 1), cmd => cmd.Version.ShouldBe(1));
            TestCommand(cgm => new MetafileVersion(cgm, 3), cmd => cmd.Version.ShouldBe(3));
        }

        [Test]
        public void MitreLimit_Write_Binary()
        {
            TestCommand(cgm => new MitreLimit(cgm, 5), cmd => cmd.Limit.ShouldBe(5));
        }

        [Test]
        public void NamePrecision_Write_Binary()
        {
            TestCommand(cgm => new NamePrecision(cgm, 8), cmd => cmd.Precision.ShouldBe(8));
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
                     cmd.SplineOrder.ShouldBe(2);
                     cmd.Points.ShouldHaveCount(2);
                     cmd.Points[0].ShouldBe(Point);
                     cmd.Points[1].ShouldBe(Point2);
                     cmd.Knots.ShouldHaveCount(4);
                     cmd.Knots[0].ShouldBe(4);
                     cmd.Knots[1].ShouldBe(6);
                     cmd.Knots[2].ShouldBe(8);
                     cmd.Knots[3].ShouldBe(33);
                     cmd.StartValue.ShouldBe(4);
                     cmd.EndValue.ShouldBe(5);
                 });
        }

        [Test]
        public void NonUniformRationalBSpline_Write_Binary()
        {
            TestCommand(cgm => new NonUniformRationalBSpline(cgm, 2, new[] { Point, Point2 }, new[] { 4.0, 6, 8, 33 }, 4, 5, new[] { 8.0, 6 }), cmd =>
             {
                 cmd.SplineOrder.ShouldBe(2);
                 cmd.Points.ShouldHaveCount(2);
                 cmd.Points[0].ShouldBe(Point);
                 cmd.Points[1].ShouldBe(Point2);
                 cmd.Knots.ShouldHaveCount(4);
                 cmd.Knots[0].ShouldBe(4);
                 cmd.Knots[1].ShouldBe(6);
                 cmd.Knots[2].ShouldBe(8);
                 cmd.Knots[3].ShouldBe(33);
                 cmd.StartValue.ShouldBe(4);
                 cmd.EndValue.ShouldBe(5);
                 cmd.Weights.ShouldHaveCount(2);
                 cmd.Weights[0].ShouldBe(8);
                 cmd.Weights[1].ShouldBe(6);
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
               cmd.IntersectionPoint.ShouldBe(Point);
               cmd.Start.ShouldBe(Point2);
               cmd.End.ShouldBe(Point3);
           });
        }

        [Test]
        public void PatternIndex_Write_Binary()
        {
            TestCommand(cgm => new PatternIndex(cgm, 8), cmd => cmd.Index.ShouldBe(8));
        }

        [Test]
        public void PatternSize_Write_Binary()
        {
            TestCommand(cgm => new PatternSize(cgm, 3, 4, 5, 6), cmd =>
               {
                   cmd.HeightX.ShouldBe(3);
                   cmd.HeightY.ShouldBe(4);
                   cmd.WidthX.ShouldBe(5);
                   cmd.WidthY.ShouldBe(6);
               });
        }

        [Test]
        public void PatternTable_Write_Binary()
        {
            TestCommand(cgm => new PatternTable(cgm, 3, 2, 1, 8, new[] { Color_Index, Color_Index2 }), cmd =>
              {
                  cmd.Index.ShouldBe(3);
                  cmd.Nx.ShouldBe(2);
                  cmd.Ny.ShouldBe(1);
                  cmd.Colors.ShouldHaveCount(2);
                  cmd.Colors[0].ShouldBe(Color_Index);
                  cmd.Colors[1].ShouldBe(Color_Index2);
              });
        }

        [Test]
        public void PickIdentifier_Write_Binary()
        {
            TestCommand(cgm => new PickIdentifier(cgm, 8), cmd => cmd.Identifier.ShouldBe(8));
        }

        [Test]
        public void PictureDirectory_Write_Binary()
        {
            var info = new PictureDirectory.PDInfo() { Identifier = "aa", Directory = 2, Location = 5 };
            var info2 = new PictureDirectory.PDInfo() { Identifier = "bbbb", Directory = 5, Location = 66 };

            TestCommand(cgm => new PictureDirectory(cgm, PictureDirectory.Type.UI32, new[] { info, info2 }), cmd =>
             {
                 cmd.Value.ShouldBe(PictureDirectory.Type.UI32);
                 cmd.Infos.ShouldHaveCount(2);
                 cmd.Infos[0].Identifier.ShouldBe(info.Identifier);
                 cmd.Infos[0].Directory.ShouldBe(info.Directory);
                 cmd.Infos[0].Location.ShouldBe(info.Location);
                 cmd.Infos[1].Identifier.ShouldBe(info2.Identifier);
                 cmd.Infos[1].Directory.ShouldBe(info2.Directory);
                 cmd.Infos[1].Location.ShouldBe(info2.Location);
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
               cmd.ContinuityIndicator.ShouldBe(1);
               cmd.Curves.ShouldHaveCount(3);

               cmd.Curves[0].ShouldHaveCount(4);
               cmd.Curves[0][0].ShouldBe(Point);
               cmd.Curves[0][1].ShouldBe(Point2);
               cmd.Curves[0][2].ShouldBe(Point3);
               cmd.Curves[0][3].ShouldBe(Point2);

               cmd.Curves[1].ShouldHaveCount(4);
               cmd.Curves[1][0].ShouldBe(Point2);
               cmd.Curves[1][1].ShouldBe(Point);
               cmd.Curves[1][2].ShouldBe(Point);
               cmd.Curves[1][3].ShouldBe(Point3);

               cmd.Curves[2].ShouldHaveCount(4);
               cmd.Curves[2][0].ShouldBe(Point);
               cmd.Curves[2][1].ShouldBe(Point);
               cmd.Curves[2][2].ShouldBe(Point);
               cmd.Curves[2][3].ShouldBe(Point2);
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
                cmd.ContinuityIndicator.ShouldBe(2);
                cmd.Curves.ShouldHaveCount(4);
                cmd.Curves[0].ShouldHaveCount(4);
                cmd.Curves[0][0].ShouldBe(Point);
                cmd.Curves[0][1].ShouldBe(Point2);
                cmd.Curves[0][2].ShouldBe(Point3);
                cmd.Curves[0][3].ShouldBe(Point2);

                cmd.Curves[1].ShouldHaveCount(3);
                cmd.Curves[1][0].ShouldBe(Point2);
                cmd.Curves[1][1].ShouldBe(Point);
                cmd.Curves[1][2].ShouldBe(Point);

                cmd.Curves[2].ShouldHaveCount(3);
                cmd.Curves[2][0].ShouldBe(Point);
                cmd.Curves[2][1].ShouldBe(Point);
                cmd.Curves[2][2].ShouldBe(Point);

                cmd.Curves[3].ShouldHaveCount(3);
                cmd.Curves[3][0].ShouldBe(Point3);
                cmd.Curves[3][1].ShouldBe(Point2);
                cmd.Curves[3][2].ShouldBe(Point2);
            });
        }

        [Test]
        public void PolygonElement_Write_Binary()
        {
            TestCommand(cgm => new PolygonElement(cgm, new[] { Point, Point2, Point2, Point3 }), cmd =>
            {
                cmd.Points.ShouldHaveCount(4);
                cmd.Points[0].ShouldBe(Point);
                cmd.Points[1].ShouldBe(Point2);
                cmd.Points[2].ShouldBe(Point2);
                cmd.Points[3].ShouldBe(Point3);
            });
        }

        [Test]
        public void PolygonSet_Write_Binary()
        {
            TestCommand(cgm => new PolygonSet(cgm, new[] { new KeyValuePair<PolygonSet.EdgeFlag, CgmPoint>(PolygonSet.EdgeFlag.CLOSEVIS, Point), new KeyValuePair<PolygonSet.EdgeFlag, CgmPoint>(PolygonSet.EdgeFlag.CLOSEINVIS, Point2) }), cmd =>
           {
               cmd.Set.ShouldHaveCount(2);
               cmd.Set[0].Key.ShouldBe(PolygonSet.EdgeFlag.CLOSEVIS);
               cmd.Set[0].Value.ShouldBe(Point);
               cmd.Set[1].Key.ShouldBe(PolygonSet.EdgeFlag.CLOSEINVIS);
               cmd.Set[1].Value.ShouldBe(Point2);
           });
        }

        [Test]
        public void Polyline_Write_Binary()
        {
            TestCommand(cgm => new Polyline(cgm, new[] { Point, Point2, Point2, Point3 }), cmd =>
            {
                cmd.Points.ShouldHaveCount(4);
                cmd.Points[0].ShouldBe(Point);
                cmd.Points[1].ShouldBe(Point2);
                cmd.Points[2].ShouldBe(Point2);
                cmd.Points[3].ShouldBe(Point3);
            });
        }

        [Test]
        public void PolyMarker_Write_Binary()
        {
            TestCommand(cgm => new PolyMarker(cgm, new[] { Point, Point2, Point2, Point3 }), cmd =>
            {
                cmd.Points.ShouldHaveCount(4);
                cmd.Points[0].ShouldBe(Point);
                cmd.Points[1].ShouldBe(Point2);
                cmd.Points[2].ShouldBe(Point2);
                cmd.Points[3].ShouldBe(Point3);
            });
        }

        [Test]
        public void PolySymbol_Write_Binary()
        {
            TestCommand(cgm => new PolySymbol(cgm, 4, new[] { Point, Point2, Point2, Point3 }), cmd =>
            {
                cmd.Index.ShouldBe(4);
                cmd.Points.ShouldHaveCount(4);
                cmd.Points[0].ShouldBe(Point);
                cmd.Points[1].ShouldBe(Point2);
                cmd.Points[2].ShouldBe(Point2);
                cmd.Points[3].ShouldBe(Point3);
            });
        }

        [Test]
        public void ProtectionRegionIndicator_Write_Binary()
        {
            TestCommand(cgm => new ProtectionRegionIndicator(cgm, 4, 6), cmd =>
            {
                cmd.Index.ShouldBe(4);
                cmd.Indicator.ShouldBe(6);
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
                cmd.FirstCorner.ShouldBe(Point2);
                cmd.SecondCorner.ShouldBe(Point3);
            });
        }

        [Test]
        public void RestorePrimitiveContext_Write_Binary()
        {
            TestCommand(cgm => new RestorePrimitiveContext(cgm, 16), cmd => cmd.Name.ShouldBe(16));
        }

        [Test]
        public void RestrictedText_Write_Binary()
        {
            TestCommand(cgm => new RestrictedText(cgm, "testdata", Point, 2, 5, false), cmd =>
            {
                cmd.Text.ShouldBe("testdata");
                cmd.Position.ShouldBe(Point);
                cmd.DeltaWidth.ShouldBe(2);
                cmd.DeltaHeight.ShouldBe(5);
                cmd.Final.ShouldBe(false);
            });
        }

        [Test]
        public void RestrictedTextType_Write_Binary()
        {
            TestCommand(cgm => new RestrictedTextType(cgm, RestrictedTextType.Type.BOXED_ALL), cmd => cmd.Value.ShouldBe(RestrictedTextType.Type.BOXED_ALL));
        }

        [Test]
        public void SavePrimitiveContext_Write_Binary()
        {
            TestCommand(cgm => new SavePrimitiveContext(cgm, 11), cmd => cmd.Name.ShouldBe(11));
        }

        [Test]
        public void ScalingMode_Write_Binary_Abstract()
        {
            TestCommand(cgm => new ScalingMode(cgm, ScalingMode.Mode.ABSTRACT, 5), cmd =>
            {
                cmd.Value.ShouldBe(ScalingMode.Mode.ABSTRACT);
                cmd.MetricScalingFactor.ShouldBe(0);
            });
        }

        [Test]
        public void ScalingMode_Write_Binary_Metric()
        {
            TestCommand(cgm => new ScalingMode(cgm, ScalingMode.Mode.METRIC, 5), cmd =>
            {
                cmd.Value.ShouldBe(ScalingMode.Mode.METRIC);
                cmd.MetricScalingFactor.ShouldBe(5);
            });
        }

        [Test]
        public void SegmentDisplayPriority_Write_Binary()
        {
            TestCommand(cgm => new SegmentDisplayPriority(cgm, 33, 5), cmd =>
            {
                cmd.Name.ShouldBe(33);
                cmd.Prio.ShouldBe(5);
            });
        }

        [Test]
        public void SegmentHighlighting_Write_Binary()
        {
            TestCommand(cgm => new SegmentHighlighting(cgm, 33, SegmentHighlighting.Highlighting.HIGHL), cmd =>
            {
                cmd.Identifier.ShouldBe(33);
                cmd.Value.ShouldBe(SegmentHighlighting.Highlighting.HIGHL);
            });
        }

        [Test]
        public void SegmentPickPriority_Write_Binary()
        {
            TestCommand(cgm => new SegmentPickPriority(cgm, 33, 5), cmd =>
            {
                cmd.Identifier.ShouldBe(33);
                cmd.Prio.ShouldBe(5);
            });
        }

        [Test]
        public void SegmentPriorityExtend_Write_Binary()
        {
            TestCommand(cgm => new SegmentPriorityExtend(cgm, 33, 5), cmd =>
            {
                cmd.Min.ShouldBe(33);
                cmd.Max.ShouldBe(5);
            });
        }

        [Test]
        public void SegmentTransformation_Write_Binary()
        {
            TestCommand(cgm => new SegmentTransformation(cgm, 33, 1, 3, 66, 2, 45, 8), cmd =>
            {
                cmd.Identifier.ShouldBe(33);
                cmd.ScaleX.ShouldBe(1);
                cmd.RotationX.ShouldBe(3);
                cmd.RotationY.ShouldBe(66);
                cmd.ScaleY.ShouldBe(2);
                cmd.TranslationX.ShouldBe(45);
                cmd.TranslationY.ShouldBe(8);
            });
        }

        [Test]
        public void SymbolColour_Write_Binary()
        {
            TestCommand(cgm => new SymbolColour(cgm, Color_Index), cmd => cmd.Color.ShouldBe(Color_Index));
        }

        [Test]
        public void SymbolLibraryIndex_Write_Binary()
        {
            TestCommand(cgm => new SymbolLibraryIndex(cgm, 33), cmd => cmd.Index.ShouldBe(33));
        }

        [Test]
        public void SymbolLibraryList_Write_Binary()
        {
            TestCommand(cgm => new SymbolLibraryList(cgm, new[] { "test1", "another test" }), cmd =>
            {
                cmd.Names.ShouldHaveCount(2);
                cmd.Names[0].ShouldBe("test1");
                cmd.Names[1].ShouldBe("another test");
            });
        }

        [Test]
        public void SymbolOrientation_Write_Binary()
        {
            TestCommand(cgm => new SymbolOrientation(cgm, 4, 7, 33, 1), cmd =>
            {
                cmd.UpX.ShouldBe(4);
                cmd.UpY.ShouldBe(7);
                cmd.BaseX.ShouldBe(33);
                cmd.BaseY.ShouldBe(1);
            });
        }

        [Test]
        public void SymbolSize_Write_Binary()
        {
            TestCommand(cgm => new SymbolSize(cgm, SymbolSize.ScaleIndicator.BOTH, 3, 6), cmd =>
            {
                cmd.Indicator.ShouldBe(SymbolSize.ScaleIndicator.BOTH);
                cmd.Width.ShouldBe(3);
                cmd.Height.ShouldBe(6);
            });
        }

        [Test]
        public void Text_Write_Binary()
        {
            TestCommand(cgm => new Text(cgm, "this is a test", Point, true), cmd =>
            {
                cmd.Text.ShouldBe("this is a test");
                cmd.Position.ShouldBe(Point);
                cmd.Final.ShouldBe(true);
            });
        }

        [Test]
        public void TextAlignment_Write_Binary()
        {
            TestCommand(cgm => new TextAlignment(cgm, TextAlignment.HorizontalAlignmentType.LEFT, TextAlignment.VerticalAlignmentType.BOTTOM, 3, 6), cmd =>
            {
                cmd.HorizontalAlignment.ShouldBe(TextAlignment.HorizontalAlignmentType.LEFT);
                cmd.VerticalAlignment.ShouldBe(TextAlignment.VerticalAlignmentType.BOTTOM);
                cmd.ContinuousHorizontalAlignment.ShouldBe(3);
                cmd.ContinuousVerticalAlignment.ShouldBe(6);
            });
        }

        [Test]
        public void TextBundleIndex_Write_Binary()
        {
            TestCommand(cgm => new TextBundleIndex(cgm, 16), cmd => cmd.Index.ShouldBe(16));
        }

        [Test]
        public void TextColour_Write_Binary()
        {
            TestCommand(cgm => new TextColour(cgm, Color_Index2), cmd => cmd.Color.ShouldBe(Color_Index2));
        }

        [Test]
        public void TextFontIndex_Write_Binary()
        {
            TestCommand(cgm => new TextFontIndex(cgm, 23), cmd => cmd.Index.ShouldBe(23));
        }

        [Test]
        public void TextPath_Write_Binary()
        {
            TestCommand(cgm => new TextPath(cgm, TextPath.Type.LEFT), cmd => cmd.Path.ShouldBe(TextPath.Type.LEFT));
        }

        [Test]
        public void TextPrecision_Write_Binary()
        {
            TestCommand(cgm => new TextPrecision(cgm, TextPrecisionType.CHAR), cmd => cmd.Value.ShouldBe(TextPrecisionType.CHAR));
            TestCommand(cgm => new TextPrecision(cgm, TextPrecisionType.STRING), cmd => cmd.Value.ShouldBe(TextPrecisionType.STRING));
        }

        [Test]
        public void TextRepresentation_Write_Binary()
        {
            TestCommand(cgm => new TextRepresentation(cgm, 2, 5, TextPrecisionType.STRING, 44, 7, Color_Index), cmd =>
            {
                cmd.BundleIndex.ShouldBe(2);
                cmd.FontIndex.ShouldBe(5);
                cmd.Precision.ShouldBe(TextPrecisionType.STRING);
                cmd.Spacing.ShouldBe(44);
                cmd.Expansion.ShouldBe(7);
                cmd.Color.ShouldBe(Color_Index);
            });
        }

        [Test]
        public void TextScoreType_Write_Binary()
        {
            TestCommand(cgm => new TextScoreType(cgm, new[] { new TextScoreType.TSInfo() { Type = 5, Indicator = true }, new TextScoreType.TSInfo() { Type = 2, Indicator = false } }), cmd =>
             {
                 cmd.Infos.ShouldHaveCount(2);
                 cmd.Infos[0].Type.ShouldBe(5);
                 cmd.Infos[0].Indicator.ShouldBe(true);
                 cmd.Infos[1].Type.ShouldBe(2);
                 cmd.Infos[1].Indicator.ShouldBe(false);
             });
        }

        [Test]
        public void Tile_Write_Binary()
        {
            var sdr = new StructuredDataRecord();
            sdr.Add(StructuredDataRecord.StructuredDataType.E, new object[] { 2 });
            sdr.Add(StructuredDataRecord.StructuredDataType.IX, new object[] { 5, 6 });
            var image = new MemoryStream(new byte[] { 1, 20, 30, 5, 45 });

            TestCommand(cgm => new Tile(cgm, CompressionType.BITMAP, 1, 8, sdr, image), cmd =>
            {
                cmd.CompressionType.ShouldBe(CompressionType.BITMAP);
                cmd.RowPaddingIndicator.ShouldBe(1);
                cmd.CellColorPrecision.ShouldBe(8);
                cmd.DataRecord.Members.ShouldHaveCount(2);
                cmd.DataRecord.Members[0].Type.ShouldBe(StructuredDataRecord.StructuredDataType.E);
                cmd.DataRecord.Members[0].Count.ShouldBe(1);
                cmd.DataRecord.Members[0].Data[0].ShouldBe(2);
                cmd.DataRecord.Members[1].Type.ShouldBe(StructuredDataRecord.StructuredDataType.IX);
                cmd.DataRecord.Members[1].Count.ShouldBe(2);
                cmd.DataRecord.Members[1].Data[0].ShouldBe(5);
                cmd.DataRecord.Members[1].Data[1].ShouldBe(6);
            });

            TestCommand(cgm => new Tile(cgm, CompressionType.PNG, 88, 16, sdr, image), cmd =>
            {
                cmd.CompressionType.ShouldBe(CompressionType.PNG);
                cmd.RowPaddingIndicator.ShouldBe(88);
                cmd.CellColorPrecision.ShouldBe(16);
                cmd.DataRecord.Members.ShouldHaveCount(2);
                cmd.DataRecord.Members[0].Type.ShouldBe(StructuredDataRecord.StructuredDataType.E);
                cmd.DataRecord.Members[1].Type.ShouldBe(StructuredDataRecord.StructuredDataType.IX);
                cmd.DataRecord.Members[1].Count.ShouldBe(2);
                cmd.DataRecord.Members[1].Data[0].ShouldBe(5);
                cmd.DataRecord.Members[1].Data[1].ShouldBe(6);
                cmd.Image.ToArray().ShouldBeEquivalentTo(image.ToArray());
            });
        }

        [Test]
        public void Transparency_Write_Binary()
        {
            TestCommand(cgm => new Transparency(cgm, true), cmd => cmd.Flag.ShouldBe(true));
        }

        [Test]
        public void TransparentCellColour_Write_Binary()
        {
            TestCommand(cgm => new TransparentCellColour(cgm, true, Color_Index), cmd =>
            {
                cmd.Indicator.ShouldBe(true);
                cmd.Color.ShouldBe(Color_Index);
            });
        }

        [Test]
        public void VDCExtent_Write_Binary()
        {
            TestCommand(cgm => new VdcExtent(cgm, Point, Point2), cmd =>
            {
                cmd.LowerLeftCorner.ShouldBe(Point);
                cmd.UpperRightCorner.ShouldBe(Point2);
            });
        }

        [Test]
        public void VDCExtent_Write_Binary_Negative()
        {
            var negativePoint = new CgmPoint(0, -0.4363);

            TestCommand(cgm =>
            {
                cgm.Commands.Add(new VdcType(cgm, VdcType.Type.Real));
                return new VdcExtent(cgm, negativePoint, Point2);
            }, cmd =>
            {
                cmd.LowerLeftCorner.ShouldBe(negativePoint);
                cmd.UpperRightCorner.ShouldBe(Point2);
            });
        }

        [Test]
        public void VDCExtent_Write_Binary_Negative2()
        {
            var negativePoint = new CgmPoint(-1.0000, -5.0825);

            TestCommand(cgm =>
            {
                cgm.Commands.Add(new VdcType(cgm, VdcType.Type.Real));
                return new VdcExtent(cgm, negativePoint, Point2);
            }, cmd =>
            {
                cmd.LowerLeftCorner.ShouldBe(negativePoint);
                cmd.UpperRightCorner.ShouldBe(Point2);
            });
        }

        [Test]
        public void VDCExtent_Write_Binary_Negative3()
        {
            var negativePoint = new CgmPoint(0.0000, -5.0);

            TestCommand(cgm =>
            {
                cgm.Commands.Add(new VdcType(cgm, VdcType.Type.Real));
                return new VdcExtent(cgm, negativePoint, Point2);
            }, cmd =>
            {
                cmd.LowerLeftCorner.ShouldBe(negativePoint);
                cmd.UpperRightCorner.ShouldBe(Point2);
            });
        }

        [Test]
        public void VDCIntegerPrecision_Write_Binary()
        {
            TestCommand(cgm => new VdcIntegerPrecision(cgm, 16), cmd => cmd.Precision.ShouldBe(16));
            TestCommand(cgm => new VdcIntegerPrecision(cgm, 24), cmd => cmd.Precision.ShouldBe(24));
        }

        [Test]
        public void VDCRealPrecision_Write_Binary()
        {
            TestCommand(cgm => new VdcRealPrecision(cgm, Precision.Fixed_32), cmd => cmd.Value == Precision.Fixed_32);
            TestCommand(cgm => new VdcRealPrecision(cgm, Precision.Fixed_64), cmd => cmd.Value == Precision.Fixed_64);
            TestCommand(cgm => new VdcRealPrecision(cgm, Precision.Floating_32), cmd => cmd.Value == Precision.Floating_32);
            TestCommand(cgm => new VdcRealPrecision(cgm, Precision.Floating_64), cmd => cmd.Value == Precision.Floating_64);
        }

        [Test]
        public void VDCType_Write_Binary()
        {
            TestCommand(cgm => new VdcType(cgm, VdcType.Type.Integer), cmd => cmd.Value == VdcType.Type.Integer);
            TestCommand(cgm => new VdcType(cgm, VdcType.Type.Real), cmd => cmd.Value == VdcType.Type.Real);
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

        private void TestCommand<TCommand>(Func<CgmFile, TCommand> commandCreator, Func<TCommand, bool> check) where TCommand : Command
        {
            var cgm = new BinaryCgmFile();
            cgm.Commands.Add(commandCreator(cgm));

            var content = cgm.GetContent();
            var binaryFile = new BinaryCgmFile(new MemoryStream(content));

            var newcommand = binaryFile.Commands.FirstOrDefault(cmd => cmd is TCommand) as TCommand;

            if (newcommand == null)
                Assert.Fail($"Parsed CGM did not contain {typeof(TCommand)} command!");

            var allMessages = string.Join(Environment.NewLine, binaryFile.Messages.Select(m => m.ToString().ToArray()));

            check(newcommand).ShouldBeTrue(allMessages);
        }

        private void TestCommand<TCommand>(Func<CgmFile, TCommand> commandCreator, Action<TCommand> assertLogic) where TCommand : Command
        {
            TestCommand<TCommand>(commandCreator, cmd => { assertLogic(cmd); return true; });
        }

        private bool IsColorIndex(CgmColor color)
        {
            return color.ColorIndex == Color_Index.ColorIndex;
        }

        private bool IsColorIndex2(CgmColor color)
        {
            return color.ColorIndex == Color_Index2.ColorIndex;
        }

    }
}
