using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using MoonCalc;

namespace MoonCalendarImage
{
    public class FontFactory
    {
        public string FontsPath { get; set; }

        public FontFamily LoadFontFamily(string fileName)
        {
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(FontsPath + fileName);
            return pfc.Families[0];
        }

        public Font LoadFont(FontFamily fontFamily, int size, FontStyle style)
        {
            return new Font(fontFamily, size, style, GraphicsUnit.Pixel);
        }
        public Font LoadFont(string familyName, int size, FontStyle style)
        {
            return new Font(familyName, size, style, GraphicsUnit.Pixel);
        }
    }

    #region Style
    public struct TitleStyle
    {
        public Font FontTitle { get; set; }
        public Color FontTitleColor { get; set; }
        public Font FontDate { get; set; }
        public Color FontDateColor { get; set; }

        public int Height
        {
            get
            {
                int height = FontDate.Height > FontTitle.Height ? FontDate.Height : FontTitle.Height;
                height += height / 2;
                return height;
            }
        }
    }

    public struct DayNameStyle
    {
        public Font Font { get; set; }
        public Color FontColor { get; set; }
    }

    public struct DayStyle
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public int BorderWidth { get; set; }
        public Color BorderColor { get; set; }

        public Color BackgroundColor { get; set; }

        public Font Font { get; set; }
        public Color FontColor { get; set; }
        public Color FontHolidayColor { get; set; }
        public Color FontOutOfMonthColor { get; set; }
    }

    public struct ChartStyle
    {
        public int Height { get; set; }

        public int BorderWidth { get; set; }
        public Color BorderColor { get; set; }

        public Color BackgroundColor { get; set; }

        public Font FontText { get; set; }
        public Color FontTextColor { get; set; }
        public Font FontTime { get; set; }
        public Color FontTimeColor { get; set; }
    }

    public struct PhaseStyle
    {
        public Font FontText { get; set; }
        public Color FontTextColor { get; set; }
    }

    public struct RemarkStyle
    {
        #region Properties
        public int BorderWidth { get; set; }
        public Color BorderColor { get; set; }

        public Color BackgroundColor { get; set; }

        public Font FontText { get; set; }
        public Color FontTextColor { get; set; }
        #endregion

        public Bitmap GetModelIcon(string imagesPath)
        {
            return (Bitmap)Image.FromFile(imagesPath + "Phase1.bmp");
        }

        public int GetMaxItemTextWidth(Graphics graphics, int groupIndex)
        {
            int length = 0;
            string text = string.Empty;
            for (int i = 0; i < RemarkList.Items[groupIndex].Length; i++)
            {
                RemarkItem remarkItem = RemarkList.Items[groupIndex][i];
                if (remarkItem.Text.Length > length)
                {
                    length = remarkItem.Text.Length;
                    text = remarkItem.Text;
                }
            }
            return (int)graphics.MeasureString(text, FontText).Width;
        }

        public int GetHeight(string imagesPath)
        {
            int maxItemsCount = 0;
            for (int i = 0; i < RemarkList.Items.Length; i++)
            {
                int itemsCount = RemarkList.Items[i].Length;
                if (itemsCount > maxItemsCount)
                    maxItemsCount = itemsCount;
            }
            Bitmap bmpModel = GetModelIcon(imagesPath);
            int itemHeight = bmpModel.Height + (bmpModel.Height / 2);
            int padding = bmpModel.Width / 2;
            return (itemHeight * maxItemsCount) + padding;
        }
    }
    #endregion

    #region Remark

    struct RemarkItem
    {
        public string IconName { get; set; }
        public string Text { get; set; }
    }

    class RemarkList
    {
        public static readonly RemarkItem[][] Items = 
        {
            new RemarkItem[]
            {
                new RemarkItem() { IconName = "Phase1", Text = "Новолуние" },
                new RemarkItem() { IconName = "Phase2", Text = "Растущая Луна" },
                new RemarkItem() { IconName = "Phase3", Text = "Полнолуние" },
                new RemarkItem() { IconName = "Phase4", Text = "Убывающая Луна" },
            },
            new RemarkItem[]
            {
                new RemarkItem() { IconName = "MoonEclipse", Text = "Лунное Затмение" },
                new RemarkItem() { IconName = "SunEclipse", Text = "Солнечное Затмение" },
                new RemarkItem() { IconName = "NewYear", Text = "Новый Лунный Год" },
            },
            new RemarkItem[]
            {
                new RemarkItem() { Text = "Первая строка - день месяца" },
                new RemarkItem() { Text = "Вторая строка - общие и индивидуальные лунные сутки" },
                new RemarkItem() { Text = "Третья строка - Луна в зодиаке" },
            },
        };
    }

    #endregion

    public class CalendarCreator
    {
        enum eChartType { LunarDay, Zodiac }

        struct Position
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int OffsetLeft { get; set; }
            public int OffsetTop { get; set; }
        }

        #region Properties
        public string ImagesPath { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public Settings Location { get; set; }
        public int BirthdayLunarDay { get; set; }

        public Color BackgroundColor { get; set; }
        public TitleStyle TitleStyle { get; set; }

        public DayNameStyle DayNameStyle { get; set; }
        public DayStyle DayStyle { get; set; }

        public ChartStyle LunarDayChartStyle { get; set; }
        public ChartStyle ZodiacChartStyle { get; set; }

        public PhaseStyle PhaseStyle { get; set; }

        public RemarkStyle RemarkStyle { get; set; }
        #endregion

        #region Constants
        const int secondsPerDay = 60 * 60 * 24;
        const int secondsPerWeek = secondsPerDay * 7;
        #endregion

        #region Dimensions
        int weeksCountOfMonth;
        int secondsPerPixel;

        int imageWidth;
        int imageHeight;
        #endregion

        #region Global
        DateTime dtStartCalendar = DateTime.MinValue;
        DateTime dtEndCalendar = DateTime.MinValue;
        List<Calculate> calcList = null;
        Graphics graphics = null;
        #endregion

        public void Save(Stream outputStream, string mimeType)
        {
            Init();
            InitCalcList();

            Bitmap bitmap = new Bitmap(imageWidth, imageHeight);
            graphics = Graphics.FromImage(bitmap);
            try
            {
                graphics.Clear(BackgroundColor);

                #region Draw

                Rectangle dimension;
                DrawTitle(out dimension);

                DrawDayNames(new Position() { Top = dimension.Bottom }, out dimension);
                DrawDays(new Position() { Top = dimension.Bottom }, out dimension);

                int chartListHeight = LunarDayChartStyle.Height + ZodiacChartStyle.Height;
                DrawChartList(new Position()
                {
                    Top = dimension.Top + (DayStyle.Height - chartListHeight),
                    OffsetTop = DayStyle.Height
                });

                DrawRemark(new Position()
                {
                    Left = 0,
                    Top = dimension.Bottom
                });

                #region Icons

                int iconAreaWidth;
                DrawPhases(new Position() 
                { 
                    Top = dimension.Top,
                    OffsetTop = DayStyle.Height
                }, 
                PhaseStyle.FontText.Height, out iconAreaWidth);

                DrawEclipses(new Position()
                {
                    Top = dimension.Top,
                    OffsetTop = DayStyle.Height
                }, 
                iconAreaWidth, out iconAreaWidth);

                DrawNewYear(new Position()
                {
                    Top = dimension.Top,
                    OffsetTop = DayStyle.Height
                },
                iconAreaWidth, out iconAreaWidth);

                #endregion

                #endregion

                ImageCodecInfo codec = null;
                foreach (ImageCodecInfo enc in ImageCodecInfo.GetImageEncoders())
                {
                    if (enc.MimeType == mimeType)
                    {
                        codec = enc;
                        break;
                    }
                }
                using (EncoderParameters ep = new EncoderParameters())
                {
                    ep.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                    bitmap.Save(outputStream, codec, ep);
                }
            }
            finally
            {
                bitmap.Dispose();
                graphics.Dispose();
            }
        }

        #region Init
        void Init()
        {
            #region Dimensions
            weeksCountOfMonth = GetWeeksCountOfMonth(Year, Month);
            secondsPerPixel = secondsPerDay / DayStyle.Width;

            imageWidth = (DayStyle.Width * 7) + DayStyle.BorderWidth;
            imageHeight =
                TitleStyle.Height +
                DayNameStyle.Font.Height +
                (DayStyle.Height * weeksCountOfMonth) +
                DayStyle.BorderWidth +
                RemarkStyle.GetHeight(ImagesPath);
            #endregion

            dtStartCalendar = GetStartCalendarDate();
            dtEndCalendar = GetEndCalendarDate();
        }

        void InitCalcList()
        {
            DateTime dt = new DateTime(Year, Month, 1);
            DateTime dtLeft = dt.AddMonths(-1);
            DateTime dtRight = dt.AddMonths(1);
            calcList = Calculate.GetList(dtLeft, Location, BirthdayLunarDay);
            calcList.AddRange(Calculate.GetList(dt, Location, BirthdayLunarDay));
            calcList.AddRange(Calculate.GetList(dtRight, Location, BirthdayLunarDay));
        }
        #endregion

        #region Helper
        int GetWeeksCountOfMonth(int year, int month)
        {
            DateTime startDateTime = new DateTime(year, month, 1);
            DateTime endDateTime = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            int count = 0;
            for (DateTime dt = startDateTime; dt <= endDateTime; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek == DayOfWeek.Sunday)
                    count++;
            }
            if (endDateTime.DayOfWeek != DayOfWeek.Sunday)
                count++;
            return count;
        }
        int GetWeekNumberOfMonth(DateTime dateTime)
        {
            int number = 0;
            DateTime startDateTime = new DateTime(dateTime.Year, dateTime.Month, 1);
            for (DateTime dt = startDateTime; dt <= dateTime; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek == DayOfWeek.Sunday)
                    number++;
            }
            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                number--;
            return number;
        }

        DateTime GetStartCalendarDate()
        {
            DateTime dateTime = new DateTime(Year, Month, 1);
            int offsetDays = (int)dateTime.DayOfWeek - 1;
            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                offsetDays = 6;
            return dateTime.AddDays(-offsetDays);
        }
        DateTime GetEndCalendarDate()
        {
            DateTime dateTime = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
            int offsetDays = 7 - (int)dateTime.DayOfWeek;
            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                offsetDays = 0;
            return dateTime.AddDays(offsetDays);
        }
        #endregion

        #region Draw

        #region Title

        void DrawTitle(out Rectangle dim)
        {
            string title = "ЛУННЫЙ КАЛЕНДАРЬ";
            SolidBrush fontTitleBrush = new SolidBrush(TitleStyle.FontTitleColor);
            graphics.DrawString(title, TitleStyle.FontTitle, fontTitleBrush, 0, 0);

            string date = Helper.GetMonthName(Month) + " " + Year.ToString();
            SolidBrush fontDateBrush = new SolidBrush(TitleStyle.FontDateColor);
            int left = imageWidth - (int)graphics.MeasureString(date, TitleStyle.FontDate).Width;
            graphics.DrawString(date, TitleStyle.FontDate, fontDateBrush, left, 0);

            dim = new Rectangle(0, 0, imageWidth, TitleStyle.Height);
        }

        #endregion

        #region Days

        void DrawDayNames(Position pos, out Rectangle dim)
        {
            SolidBrush fontBrush = new SolidBrush(DayNameStyle.FontColor);

            dim = new Rectangle();
            dim.X = pos.Left;
            dim.Y = pos.Top;
            dim.Height = DayNameStyle.Font.Height;

            for (int i = 1; i <= 7; i++)
            {
                DayOfWeek dayOfWeek = DayOfWeek.Sunday;
                if (i < 7)
                    dayOfWeek = (DayOfWeek)i;
                string dayName = Helper.GetDayName(dayOfWeek);
                dayName = dayName.Substring(0, 1).ToUpper() + dayName.Substring(1, dayName.Length - 1);
                int width = (int)graphics.MeasureString(dayName, DayNameStyle.Font).Width;
                dim.Width += width;
                int left = pos.Left + (DayStyle.Width * (i - 1)) + ((DayStyle.Width - width) / 2);
                graphics.DrawString(dayName, DayNameStyle.Font, fontBrush, left, pos.Top);
            }
        }

        void DrawDays(Position pos, out Rectangle dim)
        {
            Pen pen = new Pen(DayStyle.BorderColor, DayStyle.BorderWidth);
            SolidBrush brush = new SolidBrush(DayStyle.BackgroundColor);

            dim = new Rectangle();
            dim.X = pos.Left;
            dim.Y = pos.Top;
            dim.Width = DayStyle.Width * 7;
            dim.Height = DayStyle.Height * weeksCountOfMonth;

            DateTime dateTime = dtStartCalendar;
            for (int y = 0; y < weeksCountOfMonth; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    int left = pos.Left + (DayStyle.Width * x);
                    int top = pos.Top + (DayStyle.Height * y);

                    SolidBrush fontBrush = new SolidBrush(DayStyle.FontColor);
                    if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                        fontBrush.Color = DayStyle.FontHolidayColor;
                    if (dateTime.Month != Month)
                        fontBrush.Color = DayStyle.FontOutOfMonthColor;

                    Rectangle rect = new Rectangle(left, top, DayStyle.Width, DayStyle.Height);
                    graphics.FillRectangle(brush, rect);
                    graphics.DrawRectangle(pen, rect);
                    graphics.DrawString(dateTime.Day.ToString(), DayStyle.Font, fontBrush, left, top);

                    dateTime = dateTime.AddDays(1);
                }
            }
        }

        #endregion

        #region Charts

        void DrawChartList(Position pos)
        {
            DateTime dtStart = dtStartCalendar;
            DateTime dtEnd = dtEndCalendar.AddDays(1);
            int startIndex = calcList.FindLastIndex(c => c.dateTime <= dtStart);
            for (int calcIndex = startIndex; calcIndex < calcList.Count; calcIndex++)
            {
                Calculate calc = calcList[calcIndex];

                if (calc.dtLunarDay <= dtEnd)
                {
                    DrawChartItem(eChartType.LunarDay, calcIndex, pos);
                }
                if (calc.dtZodiac <= dtEnd)
                {
                    DrawChartItem(eChartType.Zodiac, calcIndex, new Position()
                    {
                        Top = pos.Top + LunarDayChartStyle.Height,
                        OffsetTop = pos.OffsetTop
                    });
                }
            }
        }

        void DrawChartItem(eChartType chartType, int calcIndex, Position pos)
        {
            ChartStyle chartStyle = new ChartStyle();
            DateTime dtStart = DateTime.MinValue;
            DateTime dtEnd = DateTime.MinValue;
            string captionText = string.Empty;
            string captionTime = string.Empty;

            Calculate calc = calcList[calcIndex];
            switch (chartType)
            {
                case eChartType.LunarDay:
                    chartStyle = LunarDayChartStyle;
                    dtStart = calcList[calcIndex].dtLunarDay;
                    dtEnd = calcList[calcIndex + 1].dtLunarDay;

                    captionText = calc.lunarDay.ToString();
                    if (calc.individualLunarDay > 0)
                        captionText = string.Format("{0}, {1}", calc.lunarDay, calc.individualLunarDay);
                    captionTime = calc.dtLunarDay.ToString("HH:mm");
                    break;

                case eChartType.Zodiac:
                    chartStyle = ZodiacChartStyle;
                    dtStart = calcList[calcIndex].dtZodiac;
                    dtEnd = calcList[calcIndex + 1].dtZodiac;

                    captionText = Res.GetZodiac((int)calc.zodiac);
                    captionTime = calc.dtZodiac.ToString("HH:mm");
                    break;
            }

            if (dtStart != dtEnd)
            {
                int nextTop;
                int nextWidth;
                Rectangle rect = GetChartRect(pos, chartStyle,
                    dtStart, dtEnd, out nextTop, out nextWidth);

                DrawChartRect(rect, chartStyle, captionText, captionTime, false);
                if (nextWidth > 0)
                    DrawChartRect(new Rectangle(0, nextTop, nextWidth, chartStyle.Height),
                        chartStyle, captionText, captionTime, true);
            }
        }

        Rectangle GetChartRect(Position pos, ChartStyle chartStyle, 
            DateTime dtStart, DateTime dtEnd,
            out int nextTop, out int nextWidth)
        {
            DateTime firstDay = new DateTime(Year, Month, 1);

            int numberOfWeek;
            if (dtStart.Month == Month)
                numberOfWeek = GetWeekNumberOfMonth(dtStart);
            else if (dtStart < firstDay)
                numberOfWeek = 0;
            else
                numberOfWeek = weeksCountOfMonth - 1;

            TimeSpan tsLeft = dtStart - dtStartCalendar;
            double secondsLeft = tsLeft.TotalSeconds - (numberOfWeek * secondsPerWeek);
            int left = pos.Left + Convert.ToInt32(secondsLeft / secondsPerPixel);

            TimeSpan tsWidth = dtEnd - dtStart;
            int width = Convert.ToInt32(tsWidth.TotalSeconds / secondsPerPixel) + chartStyle.BorderWidth;

            int top = pos.Top + (numberOfWeek * pos.OffsetTop);

            nextWidth = 0;
            nextTop = 0;
            if (left + width >= imageWidth)
            {
                nextWidth = (left + width + chartStyle.BorderWidth) - imageWidth;
                nextTop = pos.Top + ((numberOfWeek + 1) * pos.OffsetTop);
                width -= nextWidth;
            }

            return new Rectangle(left, top, width, chartStyle.Height);
        }

        void DrawChartRect(Rectangle rect, ChartStyle chartStyle, 
            string captionText, string captionTime, bool nextLine)
        {
            #region Style
            Pen pen = new Pen(chartStyle.BorderColor, chartStyle.BorderWidth);
            SolidBrush brush = new SolidBrush(chartStyle.BackgroundColor);

            Font fontText = chartStyle.FontText;
            SolidBrush fontBrushText = new SolidBrush(chartStyle.FontTextColor);

            Font fontTime = chartStyle.FontTime;
            SolidBrush fontBrushTime = new SolidBrush(chartStyle.FontTimeColor);
            #endregion

            #region Block
            int blockLeft = rect.Left;
            if (blockLeft < 0)
                blockLeft = 0;
            Rectangle blockRect = new Rectangle(blockLeft, rect.Top, rect.Width, rect.Height);

            graphics.FillRectangle(brush, blockRect);
            graphics.DrawRectangle(pen, blockRect);
            #endregion

            if (nextLine)
                return;

            #region Caption
            int captionTextWidth = (int)graphics.MeasureString(captionText, fontText).Width;
            int captionTimeWidth = (int)graphics.MeasureString(captionTime, fontTime).Width;
            int captionAreaWidth =
                (captionTextWidth > captionTimeWidth) ? captionTextWidth : captionTimeWidth;

            bool showRef = (captionAreaWidth > rect.Width);
            if (!showRef)
            {
                if (rect.Left < 0)
                    showRef = (captionAreaWidth > rect.Left + rect.Width);
            }

            if (!showRef)
            {
                graphics.DrawString(captionText, fontText, fontBrushText, blockRect.Left, blockRect.Top);
                graphics.DrawString(captionTime, fontTime, fontBrushTime, blockRect.Left, blockRect.Top + fontText.Height);
            }
            else
            {
                int captionTextSpaceWidth = Convert.ToInt32(captionTextWidth / captionText.Length);
                int refHeight = (fontText.Height > fontTime.Height) ? fontText.Height : fontTime.Height;
                int refWidth = captionTextWidth + captionTextSpaceWidth + captionTimeWidth;
                int refLeft = rect.Left;
                if (refLeft < 0)
                    refLeft = 0;
                if (refLeft + refWidth >= imageWidth)
                {
                    refLeft = imageWidth - refWidth - chartStyle.BorderWidth;
                }
                int refTop = rect.Top - refHeight;
                Rectangle refRect = new Rectangle(refLeft, refTop, refWidth, refHeight);

                graphics.FillRectangle(brush, refRect);
                graphics.DrawRectangle(pen, refRect);
                graphics.DrawString(captionText, fontText, fontBrushText, 
                    refRect.Left, refRect.Top);
                graphics.DrawString(captionTime, fontTime, fontBrushTime, 
                    refRect.Left + captionTextWidth + captionTextSpaceWidth, refRect.Top);
            }
            #endregion
        }

        #endregion

        #region Icons

        const int iconPaddingTop = 3;

        void DrawPhases(Position pos, int iconPaddingRight, out int iconAreaWidth)
        {
            // Список дата/фаза на месяц
            Dictionary<DateTime, Phase> dtPhases = new Dictionary<DateTime, Phase>();
            int startIndex = calcList.FindLastIndex(c => c.dateTime <= dtStartCalendar);
            DateTime dtEnd = dtEndCalendar.AddDays(1);
            for (int calcIndex = startIndex; calcIndex < calcList.Count; calcIndex++)
            {
                Calculate calc = calcList[calcIndex];
                if (calc.dtPhase <= dtEnd)
                {
                    if (!dtPhases.ContainsKey(calc.dtPhase))
                        dtPhases.Add(calc.dtPhase, calc.phase);
                }
                else
                    break;
            }

            // Изображения фаз Луны
            Bitmap[] bmps = new Bitmap[4];
            for (int i = 0; i < bmps.Length; i++)
            {
                bmps[i] = (Bitmap)Image.FromFile(
                    ImagesPath + "Phase" + (i + 1).ToString() + ".bmp");
                bmps[i].MakeTransparent();
            }

            // Шрифт подписи
            Font fontText = PhaseStyle.FontText;
            SolidBrush fontBrushText = new SolidBrush(PhaseStyle.FontTextColor);

            // Вычисляем позицию и рисуем
            int textWidth = (int)graphics.MeasureString("00:00", fontText).Width;
            iconAreaWidth = iconPaddingRight + textWidth;

            DateTime firstDay = new DateTime(Year, Month, 1);
            foreach (KeyValuePair<DateTime, Phase> dtPhase in dtPhases)
            {
                Bitmap bmp = bmps[(int)dtPhase.Value];
                string text = dtPhase.Key.ToString("HH:mm");

                #region Position

                DateTime dtStart = dtPhase.Key;
                dtStart = dtStart.Subtract(dtStart.TimeOfDay);

                int numberOfWeek;
                if (dtStart.Month == Month)
                    numberOfWeek = GetWeekNumberOfMonth(dtStart);
                else if (dtStart < firstDay)
                    numberOfWeek = 0;
                else
                    numberOfWeek = weeksCountOfMonth - 1;

                // Изображение
                TimeSpan tsLeft = dtStart - dtStartCalendar;
                double secondsLeft = tsLeft.TotalSeconds - (numberOfWeek * secondsPerWeek);
                int imgLeft = pos.Left + Convert.ToInt32(secondsLeft / secondsPerPixel);
                imgLeft += DayStyle.Width - iconPaddingRight - bmp.Width;
                int imgTop = pos.Top + (numberOfWeek * pos.OffsetTop);
                imgTop += iconPaddingTop;

                // Надпись
                int textLeft = imgLeft - ((textWidth - bmp.Width) / 2);
                int textTop = imgTop + bmp.Height;

                #endregion

                graphics.DrawImage(bmp, new Point(imgLeft, imgTop));
                graphics.DrawString(text, fontText, fontBrushText, textLeft, textTop);
            }
        }

        void DrawEclipses(Position pos, int iconPaddingRight, out int iconAreaWidth)
        {
            // Список дата/затмение на месяц
            Dictionary<DateTime, Eclipse> dtEclipses = new Dictionary<DateTime, Eclipse>();
            int startIndex = calcList.FindLastIndex(c => c.dateTime <= dtStartCalendar);
            DateTime dtEnd = dtEndCalendar.AddDays(1);
            for (int calcIndex = startIndex; calcIndex < calcList.Count; calcIndex++)
            {
                Calculate calc = calcList[calcIndex];
                if (calc.dtPhase <= dtEnd)
                {
                    bool noEclipse = (
                        calc.eclipse == Eclipse.No ||
                        calc.eclipse == Eclipse.MoonNo ||
                        calc.eclipse == Eclipse.SunNo);
                    if (!noEclipse)
                    {
                        if (!dtEclipses.ContainsKey(calc.dtPhase))
                            dtEclipses.Add(calc.dtPhase, calc.eclipse);
                    }
                }
                else
                    break;
            }

            // Изображения затмений Луны и Солнца
            Bitmap bmpMoonEclipse = (Bitmap)Image.FromFile(ImagesPath + "MoonEclipse.bmp");
            bmpMoonEclipse.MakeTransparent();
            Bitmap bmpSunEclipse = (Bitmap)Image.FromFile(ImagesPath + "SunEclipse.bmp");
            bmpSunEclipse.MakeTransparent();

            // Вычисляем позицию и рисуем
            iconAreaWidth = iconPaddingRight + bmpMoonEclipse.Width;

            DateTime firstDay = new DateTime(Year, Month, 1);
            foreach (KeyValuePair<DateTime, Eclipse> dtEclipse in dtEclipses)
            {
                Bitmap bmp = null;
                switch (dtEclipse.Value)
                {
                    case Eclipse.MoonPartial:
                    case Eclipse.MoonPartialShade:
                    case Eclipse.MoonPossiblyPartial:
                    case Eclipse.MoonPossiblyPartialShade:
                    case Eclipse.MoonPossiblyTotal:
                    case Eclipse.MoonTotal:
                        bmp = bmpMoonEclipse;
                        break;

                    case Eclipse.SunCentral:
                    case Eclipse.SunPartial:
                    case Eclipse.SunPossiblyCentral:
                    case Eclipse.SunPossilbyPartial:
                        bmp = bmpSunEclipse;
                        break;
                }

                #region Position

                DateTime dtStart = dtEclipse.Key;
                dtStart = dtStart.Subtract(dtStart.TimeOfDay);

                int numberOfWeek;
                if (dtStart.Month == Month)
                    numberOfWeek = GetWeekNumberOfMonth(dtStart);
                else if (dtStart < firstDay)
                    numberOfWeek = 0;
                else
                    numberOfWeek = weeksCountOfMonth - 1;

                TimeSpan tsLeft = dtStart - dtStartCalendar;
                double secondsLeft = tsLeft.TotalSeconds - (numberOfWeek * secondsPerWeek);
                int imgLeft = pos.Left + Convert.ToInt32(secondsLeft / secondsPerPixel);
                imgLeft += DayStyle.Width - iconPaddingRight - bmp.Width;
                int imgTop = pos.Top + (numberOfWeek * pos.OffsetTop);
                imgTop += iconPaddingTop;

                #endregion

                graphics.DrawImage(bmp, new Point(imgLeft, imgTop));
            }
        }

        void DrawNewYear(Position pos, int iconPaddingRight, out int iconAreaWidth)
        {
            iconAreaWidth = iconPaddingRight;

            Calculate calc = calcList.Find(c => c.newYear);
            if (calc == null)
                return;
            DateTime dateTime = calc.dtLunarDay;
            dateTime = dateTime.Subtract(dateTime.TimeOfDay);

            Bitmap bmp = (Bitmap)Image.FromFile(ImagesPath + "NewYear.bmp");
            bmp.MakeTransparent();

            iconAreaWidth = iconPaddingRight + bmp.Width;

            int numberOfWeek;
            if (dateTime.Month == Month)
                numberOfWeek = GetWeekNumberOfMonth(dateTime);
            else if (dateTime < new DateTime(Year, Month, 1))
                numberOfWeek = 0;
            else
                numberOfWeek = weeksCountOfMonth - 1;

            TimeSpan tsLeft = dateTime - dtStartCalendar;
            double secondsLeft = tsLeft.TotalSeconds - (numberOfWeek * secondsPerWeek);
            int imgLeft = pos.Left + Convert.ToInt32(secondsLeft / secondsPerPixel);
            imgLeft += DayStyle.Width - iconPaddingRight - bmp.Width;
            int imgTop = pos.Top + (numberOfWeek * pos.OffsetTop);
            imgTop += iconPaddingTop;

            graphics.DrawImage(bmp, new Point(imgLeft, imgTop));
        }

        #endregion

        #region Remark

        void DrawRemark(Position pos)
        {
            #region Block

            Pen blockPen = new Pen(RemarkStyle.BorderColor, RemarkStyle.BorderWidth);
            SolidBrush blockBrush = new SolidBrush(RemarkStyle.BackgroundColor);
            Rectangle blockRect = new Rectangle(
                pos.Left, pos.Top,
                imageWidth - RemarkStyle.BorderWidth, 
                RemarkStyle.GetHeight(ImagesPath));

            graphics.FillRectangle(blockBrush, blockRect);
            graphics.DrawRectangle(blockPen, blockRect);

            #endregion

            Bitmap bmpModel = RemarkStyle.GetModelIcon(ImagesPath);
            int padding = bmpModel.Width / 2;
            int textOffset = bmpModel.Width + (bmpModel.Width / 2);
            int itemHeight = bmpModel.Height + (bmpModel.Height / 2);
            int groupIndent = bmpModel.Width * 3;

            for (int groupIndex = 0; groupIndex < RemarkList.Items.Length; groupIndex++)
            {
                int left = pos.Left + padding;
                for (int gi = 0; gi < groupIndex; gi++)
                {
                    left += 
                        bmpModel.Width +
                        textOffset +
                        RemarkStyle.GetMaxItemTextWidth(graphics, gi) + 
                        groupIndent;
                }

                for (int itemIndex = 0; itemIndex < RemarkList.Items[groupIndex].Length; itemIndex++)
                {
                    int top = pos.Top + padding + (itemIndex * itemHeight);

                    RemarkItem remarkItem = RemarkList.Items[groupIndex][itemIndex];

                    if (!string.IsNullOrEmpty(remarkItem.IconName))
                    {
                        Bitmap bmp = (Bitmap)Image.FromFile(ImagesPath + remarkItem.IconName + ".bmp");
                        bmp.MakeTransparent();
                        graphics.DrawImage(bmp, new Point(left, top));
                    }

                    graphics.DrawString(remarkItem.Text,
                        RemarkStyle.FontText,
                        new SolidBrush(RemarkStyle.FontTextColor),
                        new Point(left + textOffset, top));
                }
            }
        }

        #endregion

        #endregion
    }
}
