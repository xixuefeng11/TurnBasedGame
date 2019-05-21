﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Khet.UseRule
{
    public class ClickPieceUI : UIBehavior<ClickPieceUI.UIData>, IPointerDownHandler
    {

        #region UIData

        public class UIData : ShowUI.UIData.Sub
        {

            public VP<int> keyX;

            public VP<int> keyY;

            #region Constructor

            public enum Property
            {
                keyX,
                keyY
            }

            public UIData() : base()
            {
                this.keyX = new VP<int>(this, (byte)Property.keyX, -1);
                this.keyY = new VP<int>(this, (byte)Property.keyY, -1);
            }

            #endregion

            public override Type getType()
            {
                return Type.ClickPiece;
            }

            public override bool processEvent(Event e)
            {
                bool isProcess = false;
                {
                    if (InputEvent.isEnter(e))
                    {
                        // enter
                        ClickPieceUI clickPieceUI = this.findCallBack<ClickPieceUI>();
                        if (clickPieceUI != null)
                        {
                            clickPieceUI.onEnterKey();
                        }
                        else
                        {
                            Debug.LogError("clickPieceUI null: " + this);
                        }
                        isProcess = true;
                    }
                    else if (InputEvent.isArrow(e))
                    {
                        if (this.keyX.v >= 0 && this.keyX.v < 10
                            && this.keyY.v >= 0 && this.keyY.v < 8)
                        {
                            // find new key position
                            int newKeyX = this.keyX.v;
                            int newKeyY = this.keyY.v;
                            {
                                switch (e.keyCode)
                                {
                                    case KeyCode.LeftArrow:
                                        newKeyX--;
                                        break;
                                    case KeyCode.RightArrow:
                                        newKeyX++;
                                        break;
                                    case KeyCode.UpArrow:
                                        newKeyY++;
                                        break;
                                    case KeyCode.DownArrow:
                                        newKeyY--;
                                        break;
                                    default:
                                        Debug.LogError("unknown keyCode: " + e.keyCode);
                                        break;
                                }
                            }
                            // set
                            if (newKeyX >= 0 && newKeyX < 10
                                && newKeyY >= 0 && newKeyY < 8)
                            {
                                this.keyX.v = newKeyX;
                                this.keyY.v = newKeyY;
                            }
                        }
                        else
                        {
                            // bring to lastMove
                            int lastKeyX = 5;
                            int lastKeyY = 4;
                            {
                                // find gameMove
                                GameMove gameMove = null;
                                {
                                    // Find gameData
                                    GameData gameData = null;
                                    {
                                        GameDataUI.UIData gameDataUIData = this.findDataInParent<GameDataUI.UIData>();
                                        if (gameDataUIData != null)
                                        {
                                            gameData = gameDataUIData.gameData.v.data;
                                        }
                                        else
                                        {
                                            Debug.LogError("gameDataUIData null: ");
                                        }
                                    }
                                    // Process
                                    if (gameData != null)
                                    {
                                        LastMove lastMove = gameData.lastMove.v;
                                        if (lastMove != null)
                                        {
                                            gameMove = lastMove.gameMove.v;
                                        }
                                        else
                                        {
                                            Debug.LogError("lastMove null: " + this);
                                        }
                                    }
                                    else
                                    {
                                        // Debug.LogError ("gameData null: " + data);
                                    }
                                }
                                // process
                                if (gameMove != null)
                                {
                                    switch (gameMove.getType())
                                    {
                                        case GameMove.Type.KhetMove:
                                            {
                                                // TODO Can hoan thien
                                            }
                                            break;
                                        case GameMove.Type.None:
                                            break;
                                        default:
                                            Debug.LogError("unknown type: " + gameMove.getType() + "; " + this);
                                            break;
                                    }
                                }
                                else
                                {
                                    Debug.LogError("gameMove null: " + this);
                                }
                            }
                            // set
                            this.keyX.v = lastKeyX;
                            this.keyY.v = lastKeyY;
                        }
                        isProcess = true;
                    }
                }
                return isProcess;
            }

        }

        #endregion

        public override int getStartAllocate()
        {
            return Setting.get().defaultChosenGame.v.getGame() == GameType.Type.Khet ? 1 : 0;
        }

        #region Refresh

        public GameObject keySelect;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // keySelect
                    {
                        if (keySelect != null)
                        {
                            if (this.data.keyX.v >= 0 && this.data.keyX.v < 10
                                && this.data.keyY.v >= 0 && this.data.keyY.v < 8)
                            {
                                keySelect.SetActive(true);
                                int position = (this.data.keyY.v + 1) * Common.BoardWidth + (this.data.keyX.v + 1);
                                keySelect.transform.localPosition = Common.getLocalPosition(position);
                            }
                            else
                            {
                                keySelect.SetActive(false);
                            }
                        }
                        else
                        {
                            Debug.LogError("keySelect null: " + this);
                        }
                    }
                }
                else
                {
                    // Debug.LogError ("data null: " + this);
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return false;
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                dirty = true;
                return;
            }
            Debug.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Child
                {

                }
                this.setDataNull(uiData);
                return;
            }
            Debug.LogError("Don't process: " + data + "; " + this);
        }

        public override void onUpdateSync<T>(WrapProperty wrapProperty, List<Sync<T>> syncs)
        {
            if (WrapProperty.checkError(wrapProperty))
            {
                return;
            }
            if (wrapProperty.p is UIData)
            {
                switch ((UIData.Property)wrapProperty.n)
                {
                    case UIData.Property.keyX:
                        dirty = true;
                        break;
                    case UIData.Property.keyY:
                        dirty = true;
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        #region IPointerDownHandler

        private void clickPiece(int x, int y)
        {
            if (this.data != null)
            {
                Khet khet = null;
                // Check isActive
                bool isActive = false;
                {
                    UseRuleInputUI.UIData useRuleInputUIData = this.data.findDataInParent<UseRuleInputUI.UIData>();
                    if (useRuleInputUIData != null)
                    {
                        khet = useRuleInputUIData.khet.v.data;
                        if (khet != null)
                        {
                            if (Game.IsPlaying(khet))
                            {
                                isActive = true;
                            }
                        }
                        else
                        {
                            Debug.LogError("khet null: " + this);
                            return;
                        }
                    }
                    else
                    {
                        Debug.LogError("useRuleInputUIData null: " + this);
                    }
                }
                if (isActive)
                {
                    if (x >= 0 && x < 10 && y >= 0 && y < 10)
                    {
                        int position = (y + 1) * Common.BoardWidth + (x + 1);
                        if (position >= 0 && position < khet._board.vs.Count)
                        {
                            if (khet._board.vs[position] == Common.OffBoard)
                            {
                                // offboard
                            }
                            else if (khet._board.vs[position] == Common.Empty)
                            {
                                // empty
                            }
                            else
                            {
                                Common.Player player = Common.GetOwner(khet._board.vs[position]);
                                if (player == (Common.Player)khet._playerToMove.v)
                                {
                                    ShowUI.UIData showUIData = this.data.findDataInParent<ShowUI.UIData>();
                                    if (showUIData != null)
                                    {
                                        ClickDestUI.UIData clickDestUIData = new ClickDestUI.UIData();
                                        {
                                            clickDestUIData.uid = showUIData.sub.makeId();
                                            clickDestUIData.piecePosition.v = position;
                                        }
                                        showUIData.sub.v = clickDestUIData;
                                    }
                                    else
                                    {
                                        Debug.LogError("showUIData null");
                                    }
                                }
                                else
                                {
                                    Debug.LogError("click other color: " + player);
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("position error: " + position + ", " + khet._board.vs.Count);
                        }
                    }
                    else
                    {
                        Debug.LogError("click outside board: " + x + ", " + y);
                    }
                }
                else
                {
                    Debug.LogError("not active: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        public void onEnterKey()
        {
            if (this.data != null)
            {
                this.clickPiece(this.data.keyX.v, this.data.keyY.v);
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.LogError("OnPointerDown: " + eventData);
            Vector3 localPosition = transform.InverseTransformPoint(eventData.position);
            int x = 0;
            int y = 0;
            Common.convertLocalPositionToXY(localPosition, out x, out y);
            Debug.LogError("localPosition: " + localPosition + ", " + x + ", " + y);
            this.clickPiece(x, y);
        }

        #endregion

    }
}