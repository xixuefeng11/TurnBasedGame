﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Khet.NoneRule
{
    public class ClickPosUI : UIBehavior<ClickPosUI.UIData>
    {

        #region UIData

        public class UIData : NoneRuleInputUI.UIData.Sub
        {

            public VP<int> position;

            #region Constructor

            public enum Property
            {
                position
            }

            public UIData() : base()
            {
                this.position = new VP<int>(this, (byte)Property.position, 0);
            }

            #endregion

            public override Type getType()
            {
                return Type.ClickPos;
            }

            public override bool processEvent(Event e)
            {
                bool isProcess = false;
                {
                    // back
                    if (!isProcess)
                    {
                        if (InputEvent.isBackEvent(e))
                        {
                            ClickPosUI clickPosUI = this.findCallBack<ClickPosUI>();
                            if (clickPosUI != null)
                            {
                                clickPosUI.onClickBtnBack();
                            }
                            else
                            {
                                Debug.LogError("clickPosUI null: " + this);
                            }
                            isProcess = true;
                        }
                    }
                }
                return isProcess;
            }

        }

        #endregion

        #region txt

        public Text lbTitle;

        public Button btnSetPiece;
        public Text tvSetPiece;

        public Button btnMove;
        public Text tvMove;

        public Button btnRotateAdd;
        public Text tvRotateAdd;

        public Button btnRotateSub;
        public Text tvRotateSub;

        public Button btnEndTurn;
        public Text tvEndTurn;

        public Button btnClear;
        public Text tvClear;

        private static readonly TxtLanguage txtRotateAdd = new TxtLanguage("Rotate add");
        private static readonly TxtLanguage txtRotateSub = new TxtLanguage("Rotate sub");

        public Button btnCreateByFen;
        public Text tvCreateByFen;

        static ClickPosUI()
        {
            txtRotateAdd.add(Language.Type.vi, "Quay tiến");
            txtRotateSub.add(Language.Type.vi, "Quay lùi");
        }

        #endregion

        #region Refresh

        public GameObject ivSelect;
        public Transform contentContainer;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    // imgSelect
                    {
                        if (ivSelect != null)
                        {
                            // position
                            ivSelect.transform.localPosition = Common.getLocalPosition(this.data.position.v);
                            // Scale
                            {
                                int playerView = GameDataBoardUI.UIData.getPlayerView(this.data);
                                ivSelect.transform.localScale = (playerView == 0 ? new Vector3(1f, 1f, 1f) : new Vector3(1f, -1f, 1f));
                            }
                        }
                        else
                        {
                            Debug.LogError("imgSelect null: " + this);
                        }
                    }
                    // isClickPiece
                    bool isClickPiece = false;
                    {
                        NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData>();
                        if (noneRuleInputUIData != null)
                        {
                            Khet khet = noneRuleInputUIData.khet.v.data;
                            if (khet != null)
                            {
                                if (this.data.position.v >= 0 && this.data.position.v < khet._board.vs.Count)
                                {
                                    if (khet._board.vs[this.data.position.v] != Common.OffBoard && khet._board.vs[this.data.position.v] != Common.Empty)
                                    {
                                        isClickPiece = true;
                                    }
                                }
                            }
                            else
                            {
                                Debug.LogError("khet null: " + this);
                            }
                        }
                        else
                        {
                            Debug.LogError("noneRuleInputuIData null: " + this);
                        }
                    }
                    // btn
                    if (btnMove != null && btnRotateAdd != null && btnRotateSub != null)
                    {
                        btnMove.gameObject.SetActive(isClickPiece);
                        btnRotateAdd.gameObject.SetActive(isClickPiece);
                        btnRotateSub.gameObject.SetActive(isClickPiece);
                    }
                    else
                    {
                        Debug.LogError("btnMove, btnRotateAdd, btnRotateSub null: " + this);
                    }
                    // UI
                    {
                        float deltaY = 0;
                        // header
                        deltaY += 30 + 10;
                        // btnSetPiece
                        {
                            if (btnSetPiece != null && btnSetPiece.gameObject.activeSelf)
                            {
                                UIRectTransform.SetPosY((RectTransform)btnSetPiece.transform, deltaY);
                                deltaY += 40;
                            }
                            else
                            {
                                Debug.LogError("btnSetPiece null");
                            }
                        }
                        // btnMove
                        {
                            if (btnMove != null && btnMove.gameObject.activeSelf)
                            {
                                UIRectTransform.SetPosY((RectTransform)btnMove.transform, deltaY);
                                deltaY += 40;
                            }
                            else
                            {
                                Debug.LogError("btnMove null");
                            }
                        }
                        // btnRotateAdd
                        {
                            if (btnRotateAdd != null && btnRotateAdd.gameObject.activeSelf)
                            {
                                UIRectTransform.SetPosY((RectTransform)btnRotateAdd.transform, deltaY);
                                deltaY += 40;
                            }
                            else
                            {
                                Debug.LogError("btnRotateAdd null");
                            }
                        }
                        // btnRotateSub
                        {
                            if (btnRotateSub != null && btnRotateSub.gameObject.activeSelf)
                            {
                                UIRectTransform.SetPosY((RectTransform)btnRotateSub.transform, deltaY);
                                deltaY += 40;
                            }
                            else
                            {
                                Debug.LogError("btnRotateSub null");
                            }
                        }
                        // btnEndTurn
                        {
                            if (btnEndTurn != null && btnEndTurn.gameObject.activeSelf)
                            {
                                UIRectTransform.SetPosY((RectTransform)btnEndTurn.transform, deltaY);
                                deltaY += 40;
                            }
                            else
                            {
                                Debug.LogError("btnEndTurn null");
                            }
                        }
                        // btnClear
                        {
                            if (btnClear != null && btnClear.gameObject.activeSelf)
                            {
                                UIRectTransform.SetPosY((RectTransform)btnClear.transform, deltaY);
                                deltaY += 40;
                            }
                            else
                            {
                                Debug.LogError("btnClear null");
                            }
                        }
                        // btnCreateByFen
                        {
                            if (btnCreateByFen != null && btnCreateByFen.gameObject.activeSelf)
                            {
                                UIRectTransform.SetPosY((RectTransform)btnCreateByFen.transform, deltaY);
                                deltaY += 40;
                            }
                            else
                            {
                                Debug.LogError("btnCreateByFen null");
                            }
                        }
                        // set
                        if (contentContainer != null)
                        {
                            UIRectTransform.SetHeight((RectTransform)contentContainer, deltaY);
                        }
                        else
                        {
                            Debug.LogError("contentContainer null");
                        }
                    }
                    // txt
                    {
                        if (lbTitle != null)
                        {
                            lbTitle.text = ClickPosTxt.txtClickPosTitle.get();
                            Setting.get().setTitleTextSize(lbTitle);
                        }
                        else
                        {
                            Debug.LogError("lbTitle null");
                        }
                        if (tvSetPiece != null)
                        {
                            tvSetPiece.text = ClickPosTxt.txtClickPosSetPiece.get();
                        }
                        else
                        {
                            Debug.LogError("tvSetPiece null");
                        }
                        if (tvMove != null)
                        {
                            tvMove.text = ClickPosTxt.txtClickPosMove.get();
                        }
                        else
                        {
                            Debug.LogError("tvMove null");
                        }
                        if (tvRotateAdd != null)
                        {
                            tvRotateAdd.text = txtRotateAdd.get();
                        }
                        else
                        {
                            Debug.LogError("tvMove null");
                        }
                        if (tvRotateSub != null)
                        {
                            tvRotateSub.text = txtRotateSub.get();
                        }
                        else
                        {
                            Debug.LogError("tvRotateSub null");
                        }
                        if (tvEndTurn != null)
                        {
                            tvEndTurn.text = ClickPosTxt.txtClickPosEndTurn.get();
                        }
                        else
                        {
                            Debug.LogError("tvEndTurn null");
                        }
                        if (tvClear != null)
                        {
                            tvClear.text = ClickPosTxt.txtClickPosClear.get();
                        }
                        else
                        {
                            Debug.LogError("tvClear null");
                        }
                        if (tvCreateByFen != null)
                        {
                            tvCreateByFen.text = ClickPosTxt.txtClickPosCreateByFen.get();
                        }
                        else
                        {
                            Debug.LogError("tvCreateByFen null");
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

        private GameDataBoardCheckPerspectiveChange<UIData> perspectiveChange = new GameDataBoardCheckPerspectiveChange<UIData>();
        private NoneRuleInputUI.UIData noneRuleInputUIData = null;

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Setting
                Setting.get().addCallBack(this);
                // CheckChange
                {
                    perspectiveChange.addCallBack(this);
                    perspectiveChange.setData(uiData);
                }
                // Parent
                {
                    DataUtils.addParentCallBack(uiData, this, ref this.noneRuleInputUIData);
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
            // CheckChange
            if (data is GameDataBoardCheckPerspectiveChange<UIData>)
            {
                dirty = true;
                return;
            }
            // Parent
            {
                if (data is NoneRuleInputUI.UIData)
                {
                    NoneRuleInputUI.UIData noneRuleInputUIData = data as NoneRuleInputUI.UIData;
                    // Child
                    {
                        noneRuleInputUIData.khet.allAddCallBack(this);
                    }
                    dirty = true;
                    return;
                }
                // Child
                if (data is Khet)
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
                // Setting
                Setting.get().removeCallBack(this);
                // CheckChange
                {
                    perspectiveChange.removeCallBack(this);
                    perspectiveChange.setData(null);
                }
                // Parent
                {
                    DataUtils.removeParentCallBack(uiData, this, ref this.noneRuleInputUIData);
                }
                this.setDataNull(uiData);
                return;
            }
            // Setting
            if (data is Setting)
            {
                return;
            }
            // CheckChange
            if (data is GameDataBoardCheckPerspectiveChange<UIData>)
            {
                return;
            }
            // Parent
            {
                if (data is NoneRuleInputUI.UIData)
                {
                    NoneRuleInputUI.UIData noneRuleInputUIData = data as NoneRuleInputUI.UIData;
                    // Child
                    {
                        noneRuleInputUIData.khet.allRemoveCallBack(this);
                    }
                    return;
                }
                // Child
                if (data is Khet)
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
                    case UIData.Property.position:
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
                    case Setting.Property.contentTextSize:
                        dirty = true;
                        break;
                    case Setting.Property.titleTextSize:
                        dirty = true;
                        break;
                    case Setting.Property.labelTextSize:
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
                    default:
                        Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                        break;
                }
                return;
            }
            // CheckChange
            if (wrapProperty.p is GameDataBoardCheckPerspectiveChange<UIData>)
            {
                dirty = true;
                return;
            }
            // Parent
            {
                if (wrapProperty.p is NoneRuleInputUI.UIData)
                {
                    switch ((NoneRuleInputUI.UIData.Property)wrapProperty.n)
                    {
                        case NoneRuleInputUI.UIData.Property.khet:
                            {
                                ValueChangeUtils.replaceCallBack(this, syncs);
                                dirty = true;
                            }
                            break;
                        case NoneRuleInputUI.UIData.Property.sub:
                            break;
                        default:
                            Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                            break;
                    }
                    return;
                }
                // Child
                if (wrapProperty.p is Khet)
                {
                    switch ((Khet.Property)wrapProperty.n)
                    {
                        case Khet.Property._playerToMove:
                            break;
                        case Khet.Property._checkmate:
                            break;
                        case Khet.Property._drawn:
                            break;
                        case Khet.Property._moveNumber:
                            break;
                        case Khet.Property._laser:
                            break;
                        case Khet.Property._board:
                            dirty = true;
                            break;
                        case Khet.Property._pharaohPositions:
                            break;
                        case Khet.Property.khetSub:
                            break;
                        case Khet.Property.isCustom:
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

        public void onClickBtnBack()
        {
            if (this.data != null)
            {
                NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData>();
                if (noneRuleInputUIData != null)
                {
                    ClickNoneUI.UIData clickNoneUIData = noneRuleInputUIData.sub.newOrOld<ClickNoneUI.UIData>();
                    {

                    }
                    noneRuleInputUIData.sub.v = clickNoneUIData;
                }
                else
                {
                    Debug.LogError("noneRuleInputUIData null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        public void onClickBtnSetPiece()
        {
            if (this.data != null)
            {
                NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData>();
                if (noneRuleInputUIData != null)
                {
                    SetPieceUI.UIData setPieceUIData = new SetPieceUI.UIData();
                    {
                        setPieceUIData.uid = noneRuleInputUIData.sub.makeId();
                        setPieceUIData.position.v = this.data.position.v;
                    }
                    noneRuleInputUIData.sub.v = setPieceUIData;
                }
                else
                {
                    Debug.LogError("noneRuleInputUIData null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        public void onClickBtnMove()
        {
            if (this.data != null)
            {
                NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData>();
                if (noneRuleInputUIData != null)
                {
                    ClickMoveUI.UIData clickMoveUIData = new ClickMoveUI.UIData();
                    {
                        clickMoveUIData.uid = noneRuleInputUIData.sub.makeId();
                        clickMoveUIData.position.v = this.data.position.v;
                    }
                    noneRuleInputUIData.sub.v = clickMoveUIData;
                }
                else
                {
                    Debug.LogError("noneRuleInputUIData null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        public void onClickBtnEnd()
        {
            if (this.data != null)
            {
                ClientInput clientInput = InputUI.UIData.findClientInput(this.data);
                if (clientInput != null)
                {
                    EndTurn endTurn = new EndTurn();
                    {

                    }
                    clientInput.makeSend(endTurn);
                }
                else
                {
                    Debug.LogError("clientInput null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        public void onClickBtnClear()
        {
            if (this.data != null)
            {
                ClientInput clientInput = InputUI.UIData.findClientInput(this.data);
                if (clientInput != null)
                {
                    Clear clear = new Clear();
                    {

                    }
                    clientInput.makeSend(clear);
                }
                else
                {
                    Debug.LogError("clientInput null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        public void onClickBtnRotateAdd()
        {
            if (this.data != null)
            {
                this.onClickBtnRotate(true);
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        public void onClickBtnRotateSub()
        {
            if (this.data != null)
            {
                this.onClickBtnRotate(false);
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        private void onClickBtnRotate(bool isAdd)
        {
            if (this.data != null)
            {
                ClientInput clientInput = InputUI.UIData.findClientInput(this.data);
                if (clientInput != null)
                {
                    KhetCustomRotate khetCustomRotate = new KhetCustomRotate();
                    {
                        khetCustomRotate.position.v = this.data.position.v;
                        khetCustomRotate.isAdd.v = isAdd;
                    }
                    clientInput.makeSend(khetCustomRotate);
                }
                else
                {
                    Debug.LogError("clientInput null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null: " + this);
            }
        }

        public void onClickBtnCreateByFen()
        {
            if (this.data != null)
            {
                NoneRuleInputUI.UIData noneRuleInputUIData = this.data.findDataInParent<NoneRuleInputUI.UIData>();
                if (noneRuleInputUIData != null)
                {
                    CreateByFenUI.UIData createFenUIData = new CreateByFenUI.UIData();
                    {
                        createFenUIData.uid = noneRuleInputUIData.sub.makeId();
                    }
                    noneRuleInputUIData.sub.v = createFenUIData;
                }
                else
                {
                    Debug.LogError("noneRuleInputUIData null: " + this);
                }
            }
            else
            {
                Debug.LogError("data null");
            }
        }

    }
}