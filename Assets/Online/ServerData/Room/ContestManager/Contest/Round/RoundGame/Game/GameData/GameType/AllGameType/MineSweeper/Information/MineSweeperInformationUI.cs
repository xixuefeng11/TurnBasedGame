﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MineSweeper
{
    public class MineSweeperInformationUI : UIHaveTransformDataBehavior<MineSweeperInformationUI.UIData>
    {

        #region UIData

        public class UIData : GameTypeInformationUI.UIData.Sub
        {

            public VP<ReferenceData<MineSweeper>> mineSweeper;

            public VP<UIRectTransform.ShowType> showType;

            #region Constructor

            public enum Property
            {
                mineSweeper,
                showType
            }

            public UIData() : base()
            {
                this.mineSweeper = new VP<ReferenceData<MineSweeper>>(this, (byte)Property.mineSweeper, new ReferenceData<MineSweeper>(null));
                this.showType = new VP<UIRectTransform.ShowType>(this, (byte)Property.showType, UIRectTransform.ShowType.Normal);
            }

            #endregion

            public override GameType.Type getType()
            {
                return GameType.Type.MineSweeper;
            }

            public override bool processEvent(Event e)
            {
                bool isProcess = false;
                {

                }
                return isProcess;
            }

        }

        #endregion

        #region txt, rect

        public Text lbTitle;

        public Text tvMessage;
        private static readonly TxtLanguage txtMessage = new TxtLanguage("https://en.wikipedia.org/wiki/Minesweeper_(video_game)");

        public Text lbWidth;
        public Text tvWidth;
        private static readonly TxtLanguage txtWidth = new TxtLanguage("Width");

        public Text lbHeight;
        public Text tvHeight;
        private static readonly TxtLanguage txtHeight = new TxtLanguage("Height");

        public Text lbBomb;
        public Text tvBomb;
        private static readonly TxtLanguage txtBomb = new TxtLanguage("Bomb");

        public Text lbBombFound;
        public Text tvBombFound;
        private static readonly TxtLanguage txtBombFound = new TxtLanguage("Bomb found");

        public Text lbAllowWatchBomb;
        public Toggle tgAllowWatchBomb;
        private static readonly TxtLanguage txtAllowWatchBomb = new TxtLanguage("Allow watch bomb");

        static MineSweeperInformationUI()
        {
            txtMessage.add(Language.Type.vi, "https://vi.wikipedia.org/wiki/D%C3%B2_m%C3%ACn_(tr%C3%B2_ch%C6%A1i)");
            txtWidth.add(Language.Type.vi, "Chiều rộng");
            txtHeight.add(Language.Type.vi, "Chiều dài");
            txtBomb.add(Language.Type.vi, "Số bom");
            txtBombFound.add(Language.Type.vi, "Số bom đã thấy");
            txtAllowWatchBomb.add(Language.Type.vi, "Cho phép nhìn bom");
        }

        #endregion

        #region Refresh

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    MineSweeper mineSweeper = this.data.mineSweeper.v.data;
                    if (mineSweeper != null)
                    {
                        // width
                        {
                            if (tvWidth != null)
                            {
                                tvWidth.text = "" + mineSweeper.X.v;
                                Setting.get().setContentTextSize(tvWidth);
                            }
                            else
                            {
                                Debug.LogError("tvWidth null");
                            }
                        }
                        // height
                        {
                            if (tvHeight != null)
                            {
                                tvHeight.text = "" + mineSweeper.Y.v;
                                Setting.get().setContentTextSize(tvHeight);
                            }
                            else
                            {
                                Debug.LogError("tvHeight null");
                            }
                        }
                        // bomb
                        {
                            if (tvBomb != null)
                            {
                                tvBomb.text = "" + mineSweeper.K.v;
                                Setting.get().setContentTextSize(tvBomb);
                            }
                            else
                            {
                                Debug.LogError("tvBomb null");
                            }
                        }
                        // bomb found
                        {
                            if (tvBombFound != null)
                            {
                                tvBombFound.text = ""+mineSweeper.minesFound.v;
                                Setting.get().setContentTextSize(tvBombFound);
                            }
                            else
                            {
                                Debug.LogError("tvBombFound null");
                            }
                        }
                        // allow watch bomb
                        {
                            if (tgAllowWatchBomb != null)
                            {
                                tgAllowWatchBomb.interactable = false;
                                tgAllowWatchBomb.isOn = mineSweeper.allowWatchBoomb.v;
                            }
                            else
                            {
                                Debug.LogError("tgAllowWatchBomb null");
                            }
                        }
                        // UI
                        {
                            float deltaY = 0;
                            // header
                            UIUtils.SetHeaderPosition(lbTitle, this.data.showType.v, ref deltaY);
                            // tvMessage
                            {
                                if (tvMessage != null)
                                {
                                    UIRectTransform.SetPosY(tvMessage.rectTransform, deltaY);
                                    deltaY += 30;
                                }
                                else
                                {
                                    Debug.LogError("tvMessage null");
                                }
                            }
                            // width
                            {
                                if (lbWidth != null)
                                {
                                    lbWidth.gameObject.SetActive(true);
                                    UIRectTransform.SetPosY(lbWidth.rectTransform, deltaY);
                                }
                                else
                                {
                                    Debug.LogError("lbWidth null");
                                }
                                if (tvWidth != null)
                                {
                                    UIRectTransform.SetPosY(tvWidth.rectTransform, deltaY + (UIConstants.ItemHeight - UIRectTransform.CommonTextHeight) / 2);
                                }
                                else
                                {
                                    Debug.LogError("tvWidth null");
                                }
                                deltaY += UIConstants.ItemHeight;
                            }
                            // height
                            {
                                if (lbHeight != null)
                                {
                                    lbHeight.gameObject.SetActive(true);
                                    UIRectTransform.SetPosY(lbHeight.rectTransform, deltaY);
                                }
                                else
                                {
                                    Debug.LogError("lbHeight null");
                                }
                                if (tvHeight != null)
                                {
                                    UIRectTransform.SetPosY(tvHeight.rectTransform, deltaY + (UIConstants.ItemHeight - UIRectTransform.CommonTextHeight) / 2);
                                }
                                else
                                {
                                    Debug.LogError("tvHeight null");
                                }
                                deltaY += UIConstants.ItemHeight;
                            }
                            // bomb
                            {
                                if (lbBomb != null)
                                {
                                    lbBomb.gameObject.SetActive(true);
                                    UIRectTransform.SetPosY(lbBomb.rectTransform, deltaY);
                                }
                                else
                                {
                                    Debug.LogError("lbBomb null");
                                }
                                if (tvBomb != null)
                                {
                                    UIRectTransform.SetPosY(tvBomb.rectTransform, deltaY + (UIConstants.ItemHeight - UIRectTransform.CommonTextHeight) / 2);
                                }
                                else
                                {
                                    Debug.LogError("tvBomb null");
                                }
                                deltaY += UIConstants.ItemHeight;
                            }
                            // bomb found
                            {
                                if (lbBombFound != null)
                                {
                                    lbBombFound.gameObject.SetActive(true);
                                    UIRectTransform.SetPosY(lbBombFound.rectTransform, deltaY);
                                }
                                else
                                {
                                    Debug.LogError("lbBombFound null");
                                }
                                if (tvBombFound != null)
                                {
                                    UIRectTransform.SetPosY(tvBombFound.rectTransform, deltaY + (UIConstants.ItemHeight - UIRectTransform.CommonTextHeight) / 2);
                                }
                                else
                                {
                                    Debug.LogError("tvBombFound null");
                                }
                                deltaY += UIConstants.ItemHeight;
                            }
                            // allow watch bomb
                            {
                                if (lbAllowWatchBomb != null)
                                {
                                    lbAllowWatchBomb.gameObject.SetActive(true);
                                    UIRectTransform.SetPosY(lbAllowWatchBomb.rectTransform, deltaY);
                                }
                                else
                                {
                                    Debug.LogError("lbAllowWatchBomb null");
                                }
                                if (tgAllowWatchBomb != null)
                                {
                                    UIRectTransform.SetPosY((RectTransform)tgAllowWatchBomb.transform, deltaY + (UIConstants.ItemHeight - UIRectTransform.CommonToggleHeight) / 2);
                                }
                                else
                                {
                                    Debug.LogError("tgAllowWatchBomb null");
                                }
                                deltaY += UIConstants.ItemHeight;
                            }
                            // set
                            UIRectTransform.SetHeight((RectTransform)this.transform, deltaY);
                        }
                        // txt
                        {
                            if (lbTitle != null)
                            {
                                lbTitle.text = GameType.GetStrGameType(GameType.Type.MineSweeper);
                                Setting.get().setTitleTextSize(lbTitle);
                            }
                            else
                            {
                                Debug.LogError("lbTitle null");
                            }
                            if (tvMessage != null)
                            {
                                tvMessage.text = txtMessage.get();
                                Setting.get().setContentTextSize(tvMessage);
                            }
                            else
                            {
                                Debug.LogError("tvMessage null");
                            }
                            if (lbWidth != null)
                            {
                                lbWidth.text = txtWidth.get();
                                Setting.get().setLabelTextSize(lbWidth);
                            }
                            else
                            {
                                Debug.LogError("lbWidth null");
                            }
                            if (lbHeight != null)
                            {
                                lbHeight.text = txtHeight.get();
                                Setting.get().setLabelTextSize(lbHeight);
                            }
                            else
                            {
                                Debug.LogError("lbHeight null");
                            }
                            if (lbBomb != null)
                            {
                                lbBomb.text = txtBomb.get();
                                Setting.get().setLabelTextSize(lbBomb);
                            }
                            else
                            {
                                Debug.LogError("lbBomb null");
                            }
                            if (lbBombFound != null)
                            {
                                lbBombFound.text = txtBombFound.get();
                                Setting.get().setLabelTextSize(lbBombFound);
                            }
                            else
                            {
                                Debug.LogError("lbBombFound null");
                            }
                            if (lbAllowWatchBomb != null)
                            {
                                lbAllowWatchBomb.text = txtAllowWatchBomb.get();
                                Setting.get().setLabelTextSize(lbAllowWatchBomb);
                            }
                            else
                            {
                                Debug.LogError("lbAllowWatchBomb null");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("mineSweeper null");
                    }
                }
                else
                {
                    Debug.LogError("data null");
                }
            }
        }

        public override bool isShouldDisableUpdate()
        {
            return true;
        }

        #endregion

        #region implement callBacks

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Setting
                Setting.get().addCallBack(this);
                // Child
                {
                    uiData.mineSweeper.allAddCallBack(this);
                }
                dirty = true;
                return;
            }
            // Setting
            if (data is Setting)
            {
                dirty = true;
                return;
            }
            // Child
            if (data is MineSweeper)
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
                // Setting
                Setting.get().removeCallBack(this);
                // Child
                {
                    uiData.mineSweeper.allRemoveCallBack(this);
                }
                this.setDataNull(uiData);
                return;
            }
            // Setting
            if (data is Setting)
            {
                return;
            }
            // Child
            if (data is MineSweeper)
            {
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
                    case UIData.Property.mineSweeper:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.showType:
                        dirty = true;
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Setting
            if (wrapProperty.p is Setting)
            {
                switch ((Setting.Property)wrapProperty.n)
                {
                    case Setting.Property.language:
                        dirty = true;
                        break;
                    case Setting.Property.style:
                        break;
                    case Setting.Property.showLastMove:
                        break;
                    case Setting.Property.viewUrlImage:
                        break;
                    case Setting.Property.animationSetting:
                        break;
                    case Setting.Property.maxThinkCount:
                        break;
                    case Setting.Property.defaultChosenGame:
                        break;
                    case Setting.Property.defaultRoomName:
                        break;
                    case Setting.Property.defaultChatRoomStyle:
                        break;
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // Child
            if (wrapProperty.p is MineSweeper)
            {
                switch ((MineSweeper.Property)wrapProperty.n)
                {
                    case MineSweeper.Property.Y:
                        dirty = true;
                        break;
                    case MineSweeper.Property.X:
                        dirty = true;
                        break;
                    case MineSweeper.Property.K:
                        dirty = true;
                        break;
                    case MineSweeper.Property.sub:
                        break;
                    case MineSweeper.Property.booom:
                        break;
                    case MineSweeper.Property.minesFound:
                        dirty = true;
                        break;
                    case MineSweeper.Property.init:
                        break;
                    case MineSweeper.Property.neb:
                        break;
                    case MineSweeper.Property.allowWatchBoomb:
                        dirty = true;
                        break;
                    case MineSweeper.Property.isCustom:
                        break;
                    case MineSweeper.Property.myFlags:
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

    }
}