using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace Custom.Util.AutoSaveTextEditor
{
    public class AutoSaveTextEditor : EditorWindow
    {
        #region ShowEditor()
        [MenuItem("Custom/AutoSaveTextEditor")]

        public static void ShowEditor()
        {
            string titleName = "텍스트 편집기";

            EditorWindow window = GetWindow<AutoSaveTextEditor>();

            window.titleContent = new GUIContent(titleName);
        }
        #endregion

        /* -------------------------------------------------------------------------- */
        #region Components
        private readonly string FileDefaultPath = "DefaultPath"; // EditorPrefs


        private bool _isFileExist;
        private bool _isEditMode;

        private string _filePath;
        private string _fileData;
        private string _fileTemp;

        private Vector2 _scroll;
        #endregion

        /* -------------------------------------------------------------------------- */
        // - 1
        #region FileDefatul()
        private void FileDefatul()
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fixedWidth = 100;
            style.fixedHeight = 30;

            bool btnPress = GUILayout.Button("Default Select", style);

            if(btnPress)
            {
                string filePath = EditorUtility.OpenFilePanel("Txt File Selecter", "", "txt"); 

                if(filePath != null && filePath.Length > 0 && filePath != "")
                {
                    EditorPrefs.SetString(FileDefaultPath, filePath);

                    _filePath = filePath;
                }
            }
        }
        #endregion
        #region FileDefaultView()
        private void FileDefaultView()
        {
            // style
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.fixedHeight = 30;
            style.alignment = TextAnchor.MiddleLeft;

            // logic
            string viewPath = EditorPrefs.GetString(FileDefaultPath);
            GUILayout.Label(viewPath, style);
        }
        #endregion

        // - 2
        #region FileSelect()
        private void FileSelect()
        {
            // style
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fixedWidth = 100;
            style.fixedHeight = 30;

            // logic
            bool btnPress = GUILayout.Button("File Select", style);

            if (btnPress)
            {
                if(_isEditMode)
                { 
                    if(_filePath != "")
                    { 
                        // FileSave();
                    }
                }

                string filePath = EditorUtility.OpenFilePanel("Txt File Selecter", "", "txt");;
                
                if(filePath != "")
                { 
                    _filePath = filePath;
                }
            }
        }
        #endregion
        #region FilePathView()
        private void FilePathView()
        {
            // style
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.fixedHeight = 30;
            style.alignment = TextAnchor.MiddleLeft;

            // logic
            string viewPath = _filePath;
            GUILayout.Label(viewPath, style);
        }
        #endregion
        #region FileCheckSelect()
        private void FileCheckSelect()
        {
            // style
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 13;
            style.alignment = TextAnchor.MiddleLeft;

            // logic
            string msg = " *** ";

            if (_filePath == null)
            {
                style.normal.textColor = Color.yellow;

                _isFileExist = false;
                msg += "Select Text File";
            }

            else
            {
                string[] pathSplit = _filePath.Split('.');


                if (pathSplit.Length <= 0 || _filePath == "")
                {
                    style.normal.textColor = Color.yellow;

                    _isFileExist = false;
                    msg += "Select Text File";
                }

                else
                {
                    if (pathSplit[^1] != "txt")
                    {
                        style.normal.textColor = Color.red;

                        _isFileExist = false;
                        msg += "This File Not Text File !";
                    }

                    else
                    {
                        style.normal.textColor = Color.green;

                        _isFileExist = true;
                        msg += "Text File Exist"; ;
                    }
                }
            }

            GUILayout.Label(msg, style);
        }

        #endregion

        // - 3
        #region FileEditBtn()
        private void FileEditBtn()
        {
            //
            if (!_isFileExist || _isEditMode) return;

            // style
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fixedWidth = 100;
            style.fixedHeight = 30;
            style.fontSize = 13;
            style.alignment = TextAnchor.MiddleCenter;

            // logic
            bool isEdit = GUILayout.Button("Edit", style);

            if(isEdit)
            { 
                bool exist = FileExistCheck();
            
                if(exist)
                {
                    _fileData = File.ReadAllText(_filePath);
                    _fileTemp = _fileData;
                    _isEditMode = true;
                }

                else
                { 
                    _fileData = "";
                    _fileTemp = "";
                    _isEditMode = false;
                }
            }
        }
        #endregion
        #region FileExistCheck()
        private bool FileExistCheck()
        { 
            return File.Exists(_filePath);
        }
        #endregion

        // - 4
        #region FileSaveBtn()
        private void FileSaveBtn()
        {
            if (!_isEditMode || !_isFileExist) return;

            // style
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fixedWidth = 100;
            style.fixedHeight = 30;
            style.fontSize = 13;
            style.alignment = TextAnchor.MiddleCenter;

            // logic
            bool isSave = GUILayout.Button("Save", style);
            if (isSave)
            {
                FileSave();
            }
        }
        #endregion
        #region FileEditer()
        private void FileEditer()
        { 
            if(!_isEditMode ||!_isFileExist) return;

            /* --- Notice --- */
            // style (Notice)
            GUIStyle styleNotice = new GUIStyle(GUI.skin.label);
            styleNotice.fontSize = 13;
            styleNotice.alignment = TextAnchor.MiddleLeft;
            styleNotice.normal.textColor = Color.green;

            // logic (Notice)
            string msgNotice = " *** Edit Mode";
            GUILayout.Label(msgNotice, styleNotice);
            /* ---------------*/

            /* - Text Field - */
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            _fileTemp = EditorGUILayout.TextArea(_fileTemp, GUILayout.Height(position.height - 50f));
            EditorGUILayout.EndScrollView();
            /* -------------- */
        }
        #endregion

        /* -------------------------------------------------------------------------- */
        #region DrawHorizontal()
        private void DrawHorizontal(params Action[] actions)
        { 
            EditorGUILayout.BeginHorizontal();
            foreach(Action a in actions) a.Invoke();
            EditorGUILayout.EndHorizontal();
        }
        #endregion
        #region DrawSpace()
        private void DrawSpace(int value)
        {
            for(int i = 0; i < value; i++)
            { 
                EditorGUILayout.Space();
            }
        }

        private void DrawSpace(bool id, int value)
        {
            if(!id) return;

            for (int i = 0; i < value; i++)
            {
                EditorGUILayout.Space();
            }
        }
        #endregion
        #region FileSave
        private void FileSave()
        {
            if (_filePath == null || _filePath.Length <= 0 || _filePath == "") return;
            File.WriteAllText(_filePath, _fileTemp);
        }
        #endregion

        /* -------------------------------------------------------------------------- */
        #region On GUI
        private void OnGUI()
        {
            // -1
            DrawSpace(1);
            DrawHorizontal(FileDefatul, FileDefaultView);
            
            // -2
            DrawHorizontal(FileSelect, FilePathView);
            FileCheckSelect();

            // -3
            DrawSpace(1);
            DrawHorizontal(FileEditBtn);
            
            // -4
            DrawHorizontal(FileSaveBtn);
            FileEditer();
        }
        #endregion
        #region LifeCycle
        private void OnEnable()
        {
            string path = EditorPrefs.GetString(FileDefaultPath);
            if(path == "" || path == null) return;

            _filePath = path;
            _fileData = File.ReadAllText(path);
        }

        private void OnInspectorUpdate()
        {
            if(Input.anyKey)
            { 
                Debug.Log("anyKey");
            }
        }

        private void OnDisable()
        {
            // FileSave();
        }

        private void OnDestroy()
        {
            // FileSave();
        }
        #endregion
    }
}

