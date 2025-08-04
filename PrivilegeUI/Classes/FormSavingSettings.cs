using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PrivilegeUI.Classes
{
    public static class FormSavingSettings
    {
        /// <summary>
        /// Класс-оболочка для пользовательских параметров приложения.
        /// его использование ограничено статическим классом eeSaveCW
        /// </summary>
        private class AllFGUserSettings : ApplicationSettingsBase
        {
            /// <summary>
            /// Обычно настройки приложения создаются вручную с помощью страницы свойств проекта Settings (эта страница создает файл с именем Settings.settings). 
            /// Однако настройки можно задавать и программным путем. Класс AllSizeUserSettings автоматизирует сохранение настроек. Код создан на основе примера из MSDN. 
            /// Класс расположен внутри основного статического класса, что ограничивает его видимость.
            /// Единственный параметр с именем dsAllFG типа DataSet
            /// для хранения настроек всех форм и гридов
            /// </summary>
            [UserScopedSetting()]
            [DefaultSettingValue(null)]
            public DataSet dsAllFG
            {
                get
                {
                    return ((DataSet)this["dsAllFG"]);
                }
                set
                {
                    this["dsAllFG"] = (DataSet)value;
                }
            }
        }
        /// <summary>
        /// Инициализация переменной, хранящей настройки, даже не понадобилось создавать конструктор.
        /// Хранение настроек в статическом классе
        /// </summary>
        private static AllFGUserSettings aset = new AllFGUserSettings();
        /// <summary>
        /// Сброс всех сохранённых настроек
        /// </summary>


        /// <summary>
        /// Метод GridTableName формирует имя DataTable из имен формы и DataGridView. Двойное подчеркивание (__) между ними уменьшает вероятность коллизии.
        /// Формируем имя таблицы (DataTable) из имён Формы и Грида
        /// </summary>
        /// <param name="f">Форма</param>
        /// <param name="g">Грид</param>
        /// <returns></returns>
        private static string GridTableName(Form f, DataGridView g)
        {
            return f.Name + "__" + g.Name;
        }
        private static string GridTableName(Control f, DataGridView g)
        {
            return f.Name + "__" + g.Name;
        }
        /// <summary>
        /// Метод MakeColWidthTable создаёт таблицу для хранения ширины колонок и задаёт ее структуру. Структура очень проста: имя колонки и её ширина.
        /// У результирующей таблицы есть имя, пока что она пустая и никуда не добавлена.
        /// Создание таблицы для хранения ширины колонок
        /// </summary>
        /// <param name="name">Имя таблицы</param>
        /// <returns>Готовая пустая таблица</returns>
        private static DataTable MakeCWTable(string name)
        {
            DataTable dt = new DataTable(name);
            DataColumn column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "ColName";
            dt.Columns.Add(column);
            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "ColWid";
            dt.Columns.Add(column);
            return dt;
        }
        /// <summary>
        /// Метод MakeFormTable создает таблицу, которая будет хранить местоположение, размер, состояние формы, и содержать лишь одну строку:
        /// Создание таблицы для размеров и позиции формы
        /// </summary>
        /// <param name="name">Имя (формы)</param>
        /// <returns>Готовая пустая таблица</returns>
        private static DataTable MakeFormTable(string name)
        {
            DataColumn column;
            DataTable dt = new DataTable(name);
            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "X";
            dt.Columns.Add(column);
            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "Y";
            dt.Columns.Add(column);
            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "H";
            dt.Columns.Add(column);
            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "W";
            dt.Columns.Add(column);
            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "WinSta";
            dt.Columns.Add(column);
            return dt;
        }
        /// <summary>
        /// Метод FillGrideTable обходит все колонки DataGridView и добавляет по строчке в DataTable на каждую колонку DataGridView:
        /// Заполнение таблицы грида размерами колонок
        /// </summary>
        /// <param name="dt">таблица</param>
        /// <param name="grid">грид</param>
        private static void FillGrideTable(DataTable dt, DataGridView grid)
        {
            DataRow r2 = dt.NewRow();
            r2["ColName"] = "0";
            r2["ColWid"] = grid.RowHeadersWidth;
            dt.Rows.Add(r2);
            foreach (DataGridViewColumn c in grid.Columns)
            {
                DataRow r = dt.NewRow();
                r["ColName"] = c.Name;
                r["ColWid"] = c.Width;
                dt.Rows.Add(r);
            }
            r2 = dt.NewRow();
            r2["ColName"] = "Row.Height";
            r2["ColWid"] = grid.RowTemplate.Height;
            dt.Rows.Add(r2);
        }
        /// <summary>
        /// Заполнение таблицы формы
        /// Метод FillFormSizeTable заполняет единственную строку таблицы формы. Этот метод будет вызываться только для новых форм, т.е. тех, о которых еще нет информации в настройках приложения. 
        /// В остальных случаях будет вызываться нижеследующий метод UpdateFormSizeTable, который предотвратит затирание данных о местоположении и размерах тех окон, 
        /// которые оказались максимизированными или минимизированными на момент закрытия.
        /// </summary>
        /// <param name="dt">Таблица</param>
        /// <param name="f">Форма</param>
        private static void FillFormTable(DataTable dt, Form f)
        {
            DataRow r = dt.NewRow();
            r["X"] = f.Location.X;
            r["Y"] = f.Location.Y;
            r["H"] = f.Size.Height;
            r["W"] = f.Size.Width;
            r["WinSta"] = f.WindowState;
            dt.Rows.Add(r);
        }
        /// <summary>
        /// Запись изменений в таблицу формы.
        /// Специфика обусловлена сохранением данных при максимального-минимального состояния формы
        /// </summary>
        /// <param name="dt">таблица</param>
        /// <param name="f">форма</param>
        private static void EditFormTable(DataTable dt, Form f)
        {
            DataRow r = dt.Rows[0];
            if (f.WindowState == FormWindowState.Normal)
            {
                r["X"] = f.Location.X;
                r["Y"] = f.Location.Y;
                r["H"] = f.Size.Height;
                r["W"] = f.Size.Width;
            }
            r["WinSta"] = f.WindowState;
        }
        /// <summary>
        /// Восстановление ширины колонок DataGridView. 
        /// Блоки try-catch обеспечат работоспособность при манипуляциях разработчика с набором колонок DataGridView, связанных с добавлением, удалением и переименованием колонок
        /// Восстановление ширины колонок грида
        /// </summary>
        /// <param name="dt">Таблица</param>
        /// <param name="grid">Грид</param>
        private static void RestoreGW(DataTable dt, DataGridView grid)
        {

            foreach (DataRow r in dt.Rows)
                try
                {
                    if (r.ItemArray[0].ToString() == "0") { grid.RowHeadersWidth = (int)r["ColWid"]; }
                    else
                    {
                        if (r.ItemArray[0].ToString() == "Row.Height") { grid.RowTemplate.Height = (int)r["ColWid"]; }
                        else
                        {
                            grid.Columns[(string)r["ColName"]].Width = (int)r["ColWid"];
                        }
                    }
                }
                catch { }
        }
        /// <summary>
        /// Метод RestoreForm восстанавливает всё, не глядя на состояния окна. 
        /// Местоположение и размер будут восстановлены, если пользователь переведёт максимизированное окно в нормальное состояние.
        /// Восстановление местоположения и размеры формы
        /// </summary>
        /// <param name="dt">Таблица</param>
        /// <param name="f">Грид</param>
        private static void RestoreForm(DataTable dt, Form f)
        {
            DataRow r = dt.Rows[0];
            try
            {
                f.Location = new Point((int)r["X"], (int)r["Y"]);
                f.Size = new Size((int)r["W"], (int)r["H"]);
                f.WindowState = (FormWindowState)r["WinSta"];
            }
            catch { }
        }
        /// <summary>
        /// Рассмотрим метод SaveFormGrid, главная функция сохранения. 
        /// Сигнатура метода соответствует делегату EventHandler для того, чтобы подключить в качестве обработчика события FormClosing. 
        /// При первом запуске создаётся чистый пустой DataSet, в который добавляются DataTable по мере открытия и закрытия форм. 
        /// Если настройки для формы уже есть, они редактируются, если нет – создаются. 
        /// Для сохранения ширины колонок DataGridView всегда создаётся новый DataTable, старый же удаляется, выполняется обход всех управляющих элементов формы, 
        /// и, как только обнаруживается DataGridView, формируется имя DataTable и сохраняются настройки грида.
        /// Сохранение местоположения и размеры формы и ширина всех колонок всех гридов
        /// </summary>

        private static void SearchGrid_in_Save(Control q, DataSet ds, Form f)
        {
            foreach (Control r in q.Controls)
                if (r is DataGridView)
                {
                    DataGridView grid = (DataGridView)r;
                    string name = GridTableName(f, grid);
                    if (ds.Tables.IndexOf(name) > -1) ds.Tables.Remove(name);
                    DataTable dt = MakeCWTable(name);
                    FillGrideTable(dt, grid);
                    ds.Tables.Add(dt);
                }
                else { SearchGrid_in_Save(r, ds, f); }
        }

        private static void SaveFormGrid(object sender, EventArgs e)
        {
            Form f = (Form)sender;
            DataSet ds;
            if (aset.dsAllFG != null) ds = aset.dsAllFG;
            else ds = new DataSet();
            if (ds.Tables.IndexOf(f.Name) == -1)
            {
                DataTable dt = MakeFormTable(f.Name);
                FillFormTable(dt, f);
                ds.Tables.Add(dt);
            }
            else EditFormTable(ds.Tables[f.Name], f);

            foreach (Control c in f.Controls)
                if (c is DataGridView)
                {
                    DataGridView grid = (DataGridView)c;
                    string name = GridTableName(f, grid);
                    if (ds.Tables.IndexOf(name) > -1) ds.Tables.Remove(name);
                    DataTable dt = MakeCWTable(name);
                    FillGrideTable(dt, grid);
                    ds.Tables.Add(dt);
                }
                else { SearchGrid_in_Save(c, ds, f); }
            aset.dsAllFG = ds;
            aset.Save();
        }
        /// <summary>
        /// Метод RestoreFormGrid вызывается при загрузке формы по событию Load. После восстановления размера и местоположения окна 
        /// обходятся все элементы управления в поисках управляющих элементов типа DataGridView. 
        /// Если для DataGridView найдены сохранённые значения ширина колонки, они будут восстановлены.
        /// Восстановление местоположения и размеры формы и ширина всех колонок всех гридов
        /// </summary>
        /// 
        private static void SearchGrid_in_Restore(Control q, DataSet ds, Form f)
        {
            foreach (Control r in q.Controls)
                if (r is DataGridView)
                {
                    DataGridView grid = (DataGridView)r;
                    string name = GridTableName(f, grid);
                    if (ds.Tables.IndexOf(name) > -1)
                        RestoreGW(ds.Tables[name], grid);
                }
                else { SearchGrid_in_Restore(r, ds, f); }
        }
        private static void RestoreFormGrid(object sender, EventArgs e)
        {
            if (aset.dsAllFG == null) return;
            Form f = (Form)sender;
            DataSet ds = aset.dsAllFG;
            if (ds.Tables.IndexOf(f.Name) == -1) return;
            RestoreForm(ds.Tables[f.Name], f);
            foreach (Control c in f.Controls)
                if (c is DataGridView)
                {
                    DataGridView grid = (DataGridView)c;
                    string name = GridTableName(f, grid);
                    if (ds.Tables.IndexOf(name) > -1)
                        RestoreGW(ds.Tables[name], grid);
                }
                else
                {
                    SearchGrid_in_Restore(c, ds, f);
                }
        }
        /// <summary>
        /// Наконец мы добрались до метода, который включает механизм сохранения и восстановления ширины колонок, а также местоположения и размеров форм. 
        /// В нем закрытые статические методы класса добавляются в качестве обработчиков событий.
        /// Включение механизма запоминания и восстановления.
        /// Добавляет обработку событий Load и FormClosing
        /// </summary>
        /// <param name="f">Форма (неявно)</param>
        public static void SavingOn(this Form f)
        {
            f.Load += RestoreFormGrid;
            f.FormClosing += SaveFormGrid;
        }
    }
}
