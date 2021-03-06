﻿using UnityEngine;
using UnityEngine.UI;
using frame8.Logic.Misc.Visual.UI.ScrollRectItemsAdapter;
using System.Collections;
using System.Collections.Generic;

namespace RussianDraught.UseRule
{
    public class BtnChosenMoveHolder : SriaHolderBehavior<BtnChosenMoveHolder.UIData>
    {

        #region UIData

        public class UIData : BaseItemViewsHolder
        {

            public VP<ReferenceData<RussianDraughtMove>> russianDraughtMove;

            public VP<OnClick> onClick;

            #region Constructor

            public enum Property
            {
                russianDraughtMove,
                onClick
            }

            public UIData() : base()
            {
                this.russianDraughtMove = new VP<ReferenceData<RussianDraughtMove>>(this, (byte)Property.russianDraughtMove, new ReferenceData<RussianDraughtMove>(null));
                this.onClick = new VP<OnClick>(this, (byte)Property.onClick, null);
            }

            #endregion

            public void updateView(BtnChosenMoveAdapter.UIData myParams)
            {
                // Find
                RussianDraughtMove russianDraughtMove = null;
                {
                    if (ItemIndex >= 0 && ItemIndex < myParams.russianDraughtMoves.Count)
                    {
                        russianDraughtMove = myParams.russianDraughtMoves[ItemIndex];
                    }
                    else
                    {
                        Debug.LogError("ItemIdex error: " + this);
                    }
                }
                // Update
                this.russianDraughtMove.v = new ReferenceData<RussianDraughtMove>(russianDraughtMove);
            }

            public bool processEvent(Event e)
            {
                bool isProcess = false;
                {
                    // shortKey
                    if (!isProcess)
                    {
                        if (Setting.get().useShortKey.v)
                        {
                            BtnChosenMoveHolder btnChosenMoveHolder = this.findCallBack<BtnChosenMoveHolder>();
                            if (btnChosenMoveHolder != null)
                            {
                                isProcess = btnChosenMoveHolder.useShortKey(e);
                            }
                            else
                            {
                                Debug.LogError("btnChosenMoveHolder null: " + this);
                            }
                        }
                    }
                }
                return isProcess;
            }

        }

        public interface OnClick
        {
            void onClickMove(RussianDraughtMove russianDraughtMove);
        }

        #endregion

        #region Refresh

        public Image imgPiece;

        public Text tvMove;

        public override void refresh()
        {
            base.refresh();
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    RussianDraughtMove russianDraughtMove = this.data.russianDraughtMove.v.data;
                    if (russianDraughtMove != null)
                    {
                        // imgPiece
                        {
                            if (imgPiece != null)
                            {
                                // find piece
                                int piece = Common.FREE;
                                {
                                    RussianDraughtGameDataUI.UIData russianDraughtGameDataUIData = this.data.findDataInParent<RussianDraughtGameDataUI.UIData>();
                                    if (russianDraughtGameDataUIData != null)
                                    {
                                        GameData gameData = russianDraughtGameDataUIData.gameData.v.data;
                                        if (gameData != null)
                                        {
                                            if (gameData.gameType.v != null && gameData.gameType.v is RussianDraught)
                                            {
                                                RussianDraught russianDraught = gameData.gameType.v as RussianDraught;
                                                piece = russianDraught.getPiece(russianDraughtMove.from());
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogError("gameData null: " + this);
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("russianDraughtGameDataUIData null: " + this);
                                    }
                                }
                                // set
                                {
                                    imgPiece.sprite = RussianDraughtPieceSprite.get().getSprite(piece);
                                }
                            }
                            else
                            {
                                Debug.LogError("imgPromotion null: " + this);
                            }
                        }
                        // tvMove
                        if (tvMove != null)
                        {
                            tvMove.text = Core.unityGetStrMove(russianDraughtMove, Core.CanCorrect);
                        }
                        else
                        {
                            Debug.LogError("tvMove null: " + this);
                        }
                    }
                    else
                    {
                        Debug.LogError("russianDraughtMove null: " + this);
                    }
                }
                else
                {
                    // Debug.LogError ("data null: " + this);
                }
            }
        }

        #endregion

        #region implement callBacks

        private UseRuleInputUI.UIData useRuleInputUIData = null;

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.addParentCallBack(uiData, this, ref this.useRuleInputUIData);
                }
                // Child
                {
                    uiData.russianDraughtMove.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Parent
            {
                if (data is UseRuleInputUI.UIData)
                {
                    UseRuleInputUI.UIData useRuleInputUIData = data as UseRuleInputUI.UIData;
                    // Child
                    {
                        useRuleInputUIData.russianDraught.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                if (data is RussianDraught)
                {
                    dirty = true;
                    return;
                }
            }
            // Child
            {
                if (data is RussianDraughtMove)
                {
                    dirty = true;
                    return;
                }
            }
            Debug.LogError("Don't process: " + data + "; " + this);
        }

        public override void onRemoveCallBack<T>(T data, bool isHide)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Parent
                {
                    DataUtils.removeParentCallBack(uiData, this, ref this.useRuleInputUIData);
                }
                // Child
                {
                    uiData.russianDraughtMove.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Parent
            {
                if (data is UseRuleInputUI.UIData)
                {
                    UseRuleInputUI.UIData useRuleInputUIData = data as UseRuleInputUI.UIData;
                    // Child
                    {
                        useRuleInputUIData.russianDraught.allRemoveCallBack(this);
                    }
                    return;
                }
                // RussianDraught
                if (data is RussianDraught)
                {
                    return;
                }
            }
            // Child
            {
                if (data is RussianDraughtMove)
                {
                    return;
                }
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
                    case UIData.Property.russianDraughtMove:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.onClick:
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Parent
            {
                if (wrapProperty.p is UseRuleInputUI.UIData)
                {
                    switch ((UseRuleInputUI.UIData.Property)wrapProperty.n)
                    {
                        case UseRuleInputUI.UIData.Property.russianDraught:
                            {
                                ValueChangeUtils.replaceCallBack(this, syncs);
                                dirty = true;
                            }
                            break;
                        case UseRuleInputUI.UIData.Property.state:
                            break;
                        default:
                            Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                // RussianDraught
                {
                    if (wrapProperty.p is RussianDraught)
                    {
                        switch ((RussianDraught.Property)wrapProperty.n)
                        {
                            case RussianDraught.Property.Board:
                                break;
                            case RussianDraught.Property.num_wpieces:
                                break;
                            case RussianDraught.Property.num_bpieces:
                                break;
                            case RussianDraught.Property.Color:
                                dirty = true;
                                break;
                            case RussianDraught.Property.g_root_mb:
                                break;
                            case RussianDraught.Property.realdepth:
                                break;
                            case RussianDraught.Property.RepNum:
                                break;
                            case RussianDraught.Property.HASH_KEY:
                                break;
                            case RussianDraught.Property.p_list:
                                break;
                            case RussianDraught.Property.indices:
                                break;
                            case RussianDraught.Property.g_pieces:
                                break;
                            case RussianDraught.Property.Reversible:
                                break;
                            case RussianDraught.Property.c_num:
                                break;
                            default:
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                }
            }
            // Child
            {
                if (wrapProperty.p is RussianDraughtMove)
                {
                    switch ((RussianDraughtMove.Property)wrapProperty.n)
                    {
                        case RussianDraughtMove.Property.m:
                            dirty = true;
                            break;
                        case RussianDraughtMove.Property.path:
                            dirty = true;
                            break;
                        case RussianDraughtMove.Property.l:
                            dirty = true;
                            break;
                        default:
                            Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
            }
            Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

        public override void Awake()
        {
            base.Awake();
            // OnClick
            {
                UIUtils.SetButtonOnClick(btnMove, onClickMove);
            }
        }

        public bool useShortKey(Event e)
        {
            bool isProcess = false;
            {
                if (e.isKey && e.type == EventType.KeyUp)
                {
                    switch (e.keyCode)
                    {
                        case KeyCode.M:
                            {
                                if (btnMove != null && btnMove.gameObject.activeInHierarchy && btnMove.interactable)
                                {
                                    this.onClickMove();
                                    isProcess = true;
                                }
                                else
                                {
                                    Debug.LogError("cannot click");
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            return isProcess;
        }

        public Button btnMove;

        [UnityEngine.Scripting.Preserve]
        public void onClickMove()
        {
            if (this.data != null)
            {
                RussianDraughtMove russianDraughtMove = this.data.russianDraughtMove.v.data;
                if (russianDraughtMove != null)
                {
                    if (this.data.onClick.v != null)
                    {
                        this.data.onClick.v.onClickMove(russianDraughtMove);
                    }
                    else
                    {
                        Debug.LogError("onClick null: " + this);
                    }
                }
                else
                {
                    Debug.LogError("russianDraughtMove null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

    }
}