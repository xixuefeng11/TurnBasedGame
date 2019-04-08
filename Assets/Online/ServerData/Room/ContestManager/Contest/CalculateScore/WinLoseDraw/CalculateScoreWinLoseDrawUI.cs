﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GameManager.Match
{
    public class CalculateScoreWinLoseDrawUI : UIHaveTransformDataBehavior<CalculateScoreWinLoseDrawUI.UIData>
    {

        #region UIData

        public class UIData : CalculateScore.UIData
        {

            public VP<EditData<CalculateScoreWinLoseDraw>> editCalculateScoreWinLoseDraw;

            public VP<UIRectTransform.ShowType> showType;

            #region winScore

            public VP<RequestChangeFloatUI.UIData> winScore;

            public void makeRequestChangeWinScore(RequestChangeUpdate<float>.UpdateData update, float newWinScore)
            {
                // Find
                CalculateScoreWinLoseDraw calculateScoreWinLoseDraw = null;
                {
                    EditData<CalculateScoreWinLoseDraw> editCalculateScoreWinLoseDraw = this.editCalculateScoreWinLoseDraw.v;
                    if (editCalculateScoreWinLoseDraw != null)
                    {
                        calculateScoreWinLoseDraw = editCalculateScoreWinLoseDraw.show.v.data;
                    }
                    else
                    {
                        Debug.LogError("editCalculateScoreWinLoseDraw null: " + this);
                    }
                }
                // Process
                if (calculateScoreWinLoseDraw != null)
                {
                    calculateScoreWinLoseDraw.requestChangeWinScore(Server.getProfileUserId(calculateScoreWinLoseDraw), newWinScore);
                }
                else
                {
                    Debug.LogError("haveLimit null: " + this);
                }
            }

            #endregion

            #region loseScore

            public VP<RequestChangeFloatUI.UIData> loseScore;

            public void makeRequestChangeLoseScore(RequestChangeUpdate<float>.UpdateData update, float newLoseScore)
            {
                // Find
                CalculateScoreWinLoseDraw calculateScoreWinLoseDraw = null;
                {
                    EditData<CalculateScoreWinLoseDraw> editCalculateScoreWinLoseDraw = this.editCalculateScoreWinLoseDraw.v;
                    if (editCalculateScoreWinLoseDraw != null)
                    {
                        calculateScoreWinLoseDraw = editCalculateScoreWinLoseDraw.show.v.data;
                    }
                    else
                    {
                        Debug.LogError("editCalculateScoreWinLoseDraw null: " + this);
                    }
                }
                // Process
                if (calculateScoreWinLoseDraw != null)
                {
                    calculateScoreWinLoseDraw.requestChangeLoseScore(Server.getProfileUserId(calculateScoreWinLoseDraw), newLoseScore);
                }
                else
                {
                    Debug.LogError("haveLimit null: " + this);
                }
            }

            #endregion

            #region drawScore

            public VP<RequestChangeFloatUI.UIData> drawScore;

            public void makeRequestChangeDrawScore(RequestChangeUpdate<float>.UpdateData update, float newDrawScore)
            {
                // Find
                CalculateScoreWinLoseDraw calculateScoreWinLoseDraw = null;
                {
                    EditData<CalculateScoreWinLoseDraw> editCalculateScoreWinLoseDraw = this.editCalculateScoreWinLoseDraw.v;
                    if (editCalculateScoreWinLoseDraw != null)
                    {
                        calculateScoreWinLoseDraw = editCalculateScoreWinLoseDraw.show.v.data;
                    }
                    else
                    {
                        Debug.LogError("editCalculateScoreWinLoseDraw null: " + this);
                    }
                }
                // Process
                if (calculateScoreWinLoseDraw != null)
                {
                    calculateScoreWinLoseDraw.requestChangeDrawScore(Server.getProfileUserId(calculateScoreWinLoseDraw), newDrawScore);
                }
                else
                {
                    Debug.LogError("haveLimit null: " + this);
                }
            }

            #endregion

            #region Constructor

            public enum Property
            {
                editCalculateScoreWinLoseDraw,
                showType,
                winScore,
                loseScore,
                drawScore
            }

            public UIData() : base()
            {
                this.editCalculateScoreWinLoseDraw = new VP<EditData<CalculateScoreWinLoseDraw>>(this, (byte)Property.editCalculateScoreWinLoseDraw, new EditData<CalculateScoreWinLoseDraw>());
                this.showType = new VP<UIRectTransform.ShowType>(this, (byte)Property.showType, UIRectTransform.ShowType.Normal);
                // winScore
                {
                    this.winScore = new VP<RequestChangeFloatUI.UIData>(this, (byte)Property.winScore, new RequestChangeFloatUI.UIData());
                    this.winScore.v.updateData.v.request.v = makeRequestChangeWinScore;
                }
                // loseScore
                {
                    this.loseScore = new VP<RequestChangeFloatUI.UIData>(this, (byte)Property.loseScore, new RequestChangeFloatUI.UIData());
                    this.loseScore.v.updateData.v.request.v = makeRequestChangeLoseScore;
                }
                // drawScore
                {
                    this.drawScore = new VP<RequestChangeFloatUI.UIData>(this, (byte)Property.drawScore, new RequestChangeFloatUI.UIData());
                    this.drawScore.v.updateData.v.request.v = makeRequestChangeDrawScore;
                }
            }

            #endregion

            public override CalculateScore.Type getType()
            {
                return CalculateScore.Type.WinLoseDraw;
            }

        }

        #endregion

        #region txt

        public Text lbTitle;
        private static readonly TxtLanguage txtTitle = new TxtLanguage("Calculate score by win, lose, draw score");

        public Text lbWinScore;
        private static readonly TxtLanguage txtWinScore = new TxtLanguage("Win score");

        public Text lbLoseScore;
        private static readonly TxtLanguage txtLoseScore = new TxtLanguage("Lose score");

        public Text lbDrawScore;
        private static readonly TxtLanguage txtDrawScore = new TxtLanguage("Draw score");

        static CalculateScoreWinLoseDrawUI()
        {
            // txt
            {
                txtTitle.add(Language.Type.vi, "Tính điểm theo thắng, thua, hoà");
                txtWinScore.add(Language.Type.vi, "Điểm thắng");
                txtLoseScore.add(Language.Type.vi, "Điểm thua");
                txtDrawScore.add(Language.Type.vi, "Điểm hoà");
            }
        }

        #endregion

        #region Refresh

        private bool needReset = true;

        public override void refresh()
        {
            if (dirty)
            {
                dirty = false;
                if (this.data != null)
                {
                    EditData<CalculateScoreWinLoseDraw> editCalculateScoreWinLoseDraw = this.data.editCalculateScoreWinLoseDraw.v;
                    if (editCalculateScoreWinLoseDraw != null)
                    {
                        editCalculateScoreWinLoseDraw.update();
                        // get show
                        CalculateScoreWinLoseDraw show = editCalculateScoreWinLoseDraw.show.v.data;
                        CalculateScoreWinLoseDraw compare = editCalculateScoreWinLoseDraw.compare.v.data;
                        if (show != null)
                        {
                            // different
                            if (lbTitle != null)
                            {
                                bool isDifferent = false;
                                {
                                    if (editCalculateScoreWinLoseDraw.compareOtherType.v.data != null)
                                    {
                                        if (editCalculateScoreWinLoseDraw.compareOtherType.v.data.GetType() != show.GetType())
                                        {
                                            isDifferent = true;
                                        }
                                    }
                                }
                                lbTitle.color = isDifferent ? UIConstants.DifferentIndicatorColor : UIConstants.NormalTitleColor;
                            }
                            else
                            {
                                Debug.LogError("lbTitle null: " + this);
                            }
                            // request
                            {
                                // get server state
                                Server.State.Type serverState = Server.State.Type.Connect;
                                {
                                    Server server = show.findDataInParent<Server>();
                                    if (server != null)
                                    {
                                        if (server.state.v != null)
                                        {
                                            serverState = server.state.v.getType();
                                        }
                                        else
                                        {
                                            Debug.LogError("server state null: " + this);
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("server null: " + this);
                                    }
                                }
                                // set origin
                                {
                                    // winScore
                                    {
                                        RequestChangeFloatUI.UIData winScore = this.data.winScore.v;
                                        if (winScore != null)
                                        {
                                            // update
                                            RequestChangeUpdate<float>.UpdateData updateData = winScore.updateData.v;
                                            if (updateData != null)
                                            {
                                                updateData.origin.v = show.winScore.v;
                                                updateData.canRequestChange.v = editCalculateScoreWinLoseDraw.canEdit.v;
                                                updateData.serverState.v = serverState;
                                            }
                                            else
                                            {
                                                Debug.LogError("updateData null: " + this);
                                            }
                                            // compare
                                            {
                                                if (compare != null)
                                                {
                                                    winScore.showDifferent.v = true;
                                                    winScore.compare.v = compare.winScore.v;
                                                }
                                                else
                                                {
                                                    winScore.showDifferent.v = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogError("winScore null: " + this);
                                        }
                                    }
                                    // loseScore
                                    {
                                        RequestChangeFloatUI.UIData loseScore = this.data.loseScore.v;
                                        if (loseScore != null)
                                        {
                                            // update
                                            RequestChangeUpdate<float>.UpdateData updateData = loseScore.updateData.v;
                                            if (updateData != null)
                                            {
                                                updateData.origin.v = show.loseScore.v;
                                                updateData.canRequestChange.v = editCalculateScoreWinLoseDraw.canEdit.v;
                                                updateData.serverState.v = serverState;
                                            }
                                            else
                                            {
                                                Debug.LogError("updateData null: " + this);
                                            }
                                            // compare
                                            {
                                                if (compare != null)
                                                {
                                                    loseScore.showDifferent.v = true;
                                                    loseScore.compare.v = compare.loseScore.v;
                                                }
                                                else
                                                {
                                                    loseScore.showDifferent.v = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogError("loseScore null: " + this);
                                        }
                                    }
                                    // drawScore
                                    {
                                        RequestChangeFloatUI.UIData drawScore = this.data.drawScore.v;
                                        if (drawScore != null)
                                        {
                                            // update
                                            RequestChangeUpdate<float>.UpdateData updateData = drawScore.updateData.v;
                                            if (updateData != null)
                                            {
                                                updateData.origin.v = show.drawScore.v;
                                                updateData.canRequestChange.v = editCalculateScoreWinLoseDraw.canEdit.v;
                                                updateData.serverState.v = serverState;
                                            }
                                            else
                                            {
                                                Debug.LogError("updateData null: " + this);
                                            }
                                            // compare
                                            {
                                                if (compare != null)
                                                {
                                                    drawScore.showDifferent.v = true;
                                                    drawScore.compare.v = compare.drawScore.v;
                                                }
                                                else
                                                {
                                                    drawScore.showDifferent.v = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogError("drawScore null: " + this);
                                        }
                                    }
                                }
                                // reset
                                if (needReset)
                                {
                                    needReset = false;
                                    // winScore
                                    {
                                        RequestChangeFloatUI.UIData winScore = this.data.winScore.v;
                                        if (winScore != null)
                                        {
                                            // update
                                            RequestChangeUpdate<float>.UpdateData updateData = winScore.updateData.v;
                                            if (updateData != null)
                                            {
                                                updateData.current.v = show.winScore.v;
                                                updateData.changeState.v = Data.ChangeState.None;
                                            }
                                            else
                                            {
                                                Debug.LogError("updateData null: " + this);
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogError("winScore null: " + this);
                                        }
                                    }
                                    // loseScore
                                    {
                                        RequestChangeFloatUI.UIData loseScore = this.data.loseScore.v;
                                        if (loseScore != null)
                                        {
                                            // update
                                            RequestChangeUpdate<float>.UpdateData updateData = loseScore.updateData.v;
                                            if (updateData != null)
                                            {
                                                updateData.current.v = show.loseScore.v;
                                                updateData.changeState.v = Data.ChangeState.None;
                                            }
                                            else
                                            {
                                                Debug.LogError("updateData null: " + this);
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogError("loseScore null: " + this);
                                        }
                                    }
                                    // drawScore
                                    {
                                        RequestChangeFloatUI.UIData drawScore = this.data.drawScore.v;
                                        if (drawScore != null)
                                        {
                                            // update
                                            RequestChangeUpdate<float>.UpdateData updateData = drawScore.updateData.v;
                                            if (updateData != null)
                                            {
                                                updateData.current.v = show.drawScore.v;
                                                updateData.changeState.v = Data.ChangeState.None;
                                            }
                                            else
                                            {
                                                Debug.LogError("updateData null: " + this);
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogError("drawScore null: " + this);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("show null: " + this);
                        }
                        // UI
                        {
                            float deltaY = 0;
                            // header
                            {
                                switch (this.data.showType.v)
                                {
                                    case UIRectTransform.ShowType.Normal:
                                        {
                                            if (lbTitle != null)
                                            {
                                                lbTitle.gameObject.SetActive(true);
                                            }
                                            else
                                            {
                                                Debug.LogError("lbTitle null");
                                            }
                                            deltaY += UIConstants.HeaderHeight;
                                        }
                                        break;
                                    case UIRectTransform.ShowType.HeadLess:
                                        {
                                            if (lbTitle != null)
                                            {
                                                lbTitle.gameObject.SetActive(false);
                                            }
                                            else
                                            {
                                                Debug.LogError("lbTitle null");
                                            }
                                        }
                                        break;
                                    default:
                                        Debug.LogError("unknown type: " + this.data.showType.v);
                                        break;
                                }
                            }
                            // winScore
                            {
                                if (this.data.winScore.v != null)
                                {
                                    if (lbWinScore != null)
                                    {
                                        UIRectTransform.SetPosY(lbWinScore.rectTransform, deltaY);
                                        lbWinScore.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        Debug.LogError("lbWinScore null");
                                    }
                                    UIRectTransform.SetPosY(this.data.winScore.v, deltaY + (UIConstants.ItemHeight - UIConstants.RequestHeight) / 2.0f);
                                    deltaY += UIConstants.ItemHeight;
                                }
                                else
                                {
                                    if (lbWinScore != null)
                                    {
                                        lbWinScore.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        Debug.LogError("lbWinScore null");
                                    }
                                }
                            }
                            // loseScore
                            {
                                if (this.data.loseScore.v != null)
                                {
                                    if (lbLoseScore != null)
                                    {
                                        UIRectTransform.SetPosY(lbLoseScore.rectTransform, deltaY);
                                        lbLoseScore.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        Debug.LogError("lbLoseScore null");
                                    }
                                    UIRectTransform.SetPosY(this.data.loseScore.v, deltaY + (UIConstants.ItemHeight - UIConstants.RequestHeight) / 2.0f);
                                    deltaY += UIConstants.ItemHeight;
                                }
                                else
                                {
                                    if (lbLoseScore != null)
                                    {
                                        lbLoseScore.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        Debug.LogError("lbLoseScore null");
                                    }
                                }
                            }
                            // drawScore
                            {
                                if (this.data.drawScore.v != null)
                                {
                                    if (lbDrawScore != null)
                                    {
                                        UIRectTransform.SetPosY(lbDrawScore.rectTransform, deltaY);
                                        lbDrawScore.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        Debug.LogError("lbDrawScore null");
                                    }
                                    UIRectTransform.SetPosY(this.data.drawScore.v, deltaY + (UIConstants.ItemHeight - UIConstants.RequestHeight) / 2.0f);
                                    deltaY += UIConstants.ItemHeight;
                                }
                                else
                                {
                                    if (lbDrawScore != null)
                                    {
                                        lbDrawScore.gameObject.SetActive(false);
                                    }
                                    else
                                    {
                                        Debug.LogError("lbDrawScore null");
                                    }
                                }
                            }
                            // set
                            UIRectTransform.SetHeight((RectTransform)this.transform, deltaY);
                        }
                        // txt
                        {
                            if (lbTitle != null)
                            {
                                lbTitle.text = txtTitle.get();
                                Setting.get().setTitleTextSize(lbTitle);
                            }
                            else
                            {
                                Debug.LogError("lbTitle null");
                            }
                            if (lbWinScore != null)
                            {
                                lbWinScore.text = txtWinScore.get();
                                Setting.get().setLabelTextSize(lbWinScore);
                            }
                            else
                            {
                                Debug.LogError("lbWinScore null");
                            }
                            if (lbLoseScore != null)
                            {
                                lbLoseScore.text = txtLoseScore.get();
                                Setting.get().setLabelTextSize(lbLoseScore);
                            }
                            else
                            {
                                Debug.LogError("lbLoseScore null");
                            }
                            if (lbDrawScore != null)
                            {
                                lbDrawScore.text = txtDrawScore.get();
                                Setting.get().setLabelTextSize(lbDrawScore);
                            }
                            else
                            {
                                Debug.LogError("lbDrawScore null");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("editCalculateScoreWinLoseScore null: " + this);
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
            return true;
        }

        #endregion

        #region implement callBacks

        public RequestChangeFloatUI requestFloatPrefab;

        private Server server = null;

        public override void onAddCallBack<T>(T data)
        {
            if (data is UIData)
            {
                UIData uiData = data as UIData;
                // Setting
                Setting.get().addCallBack(this);
                // Child
                {
                    uiData.editCalculateScoreWinLoseDraw.allAddCallBack(this);
                    uiData.winScore.allAddCallBack(this);
                    uiData.loseScore.allAddCallBack(this);
                    uiData.drawScore.allAddCallBack(this);
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
            {
                // editCalculateScoreWinLoseDraw
                {
                    if (data is EditData<CalculateScoreWinLoseDraw>)
                    {
                        EditData<CalculateScoreWinLoseDraw> editCalculateScoreWinLoseDraw = data as EditData<CalculateScoreWinLoseDraw>;
                        // Child
                        {
                            editCalculateScoreWinLoseDraw.show.allAddCallBack(this);
                            editCalculateScoreWinLoseDraw.compare.allAddCallBack(this);
                        }
                        dirty = true;
                        return;
                    }
                    // Child
                    {
                        if (data is CalculateScoreWinLoseDraw)
                        {
                            CalculateScoreWinLoseDraw calculateScoreWinLoseDraw = data as CalculateScoreWinLoseDraw;
                            // Parent
                            {
                                DataUtils.addParentCallBack(calculateScoreWinLoseDraw, this, ref this.server);
                            }
                            needReset = true;
                            dirty = true;
                            return;
                        }
                        // Parent
                        {
                            if (data is Server)
                            {
                                dirty = true;
                                return;
                            }
                        }
                    }
                }
                // winScore, loseScore, drawScore
                if (data is RequestChangeFloatUI.UIData)
                {
                    RequestChangeFloatUI.UIData requestChange = data as RequestChangeFloatUI.UIData;
                    // UI
                    {
                        WrapProperty wrapProperty = requestChange.p;
                        if (wrapProperty != null)
                        {
                            switch ((UIData.Property)wrapProperty.n)
                            {
                                case UIData.Property.winScore:
                                    UIUtils.Instantiate(requestChange, requestFloatPrefab, this.transform, UIConstants.RequestRect);
                                    break;
                                case UIData.Property.loseScore:
                                    UIUtils.Instantiate(requestChange, requestFloatPrefab, this.transform, UIConstants.RequestRect);
                                    break;
                                case UIData.Property.drawScore:
                                    UIUtils.Instantiate(requestChange, requestFloatPrefab, this.transform, UIConstants.RequestRect);
                                    break;
                                default:
                                    Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                    break;
                            }
                        }
                        else
                        {
                            Debug.LogError("wrapProperty null: " + this);
                        }
                    }
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
                // Child
                {
                    uiData.editCalculateScoreWinLoseDraw.allRemoveCallBack(this);
                    uiData.winScore.allRemoveCallBack(this);
                    uiData.loseScore.allRemoveCallBack(this);
                    uiData.drawScore.allRemoveCallBack(this);
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
            {
                // editCalculateScoreWinLoseDraw
                {
                    if (data is EditData<CalculateScoreWinLoseDraw>)
                    {
                        EditData<CalculateScoreWinLoseDraw> editCalculateScoreWinLoseDraw = data as EditData<CalculateScoreWinLoseDraw>;
                        // Child
                        {
                            editCalculateScoreWinLoseDraw.show.allRemoveCallBack(this);
                            editCalculateScoreWinLoseDraw.compare.allRemoveCallBack(this);
                        }
                        return;
                    }
                    // Child
                    {
                        if (data is CalculateScoreWinLoseDraw)
                        {
                            CalculateScoreWinLoseDraw calculateScoreWinLoseDraw = data as CalculateScoreWinLoseDraw;
                            // Parent
                            {
                                DataUtils.removeParentCallBack(calculateScoreWinLoseDraw, this, ref this.server);
                            }
                            return;
                        }
                        // Parent
                        {
                            if (data is Server)
                            {
                                return;
                            }
                        }
                    }
                }
                // winScore, loseScore, drawScore
                if (data is RequestChangeFloatUI.UIData)
                {
                    RequestChangeFloatUI.UIData requestChange = data as RequestChangeFloatUI.UIData;
                    // UI
                    {
                        requestChange.removeCallBackAndDestroy(typeof(RequestChangeFloatUI));
                    }
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
                    case UIData.Property.editCalculateScoreWinLoseDraw:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.showType:
                        dirty = true;
                        break;
                    case UIData.Property.winScore:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.loseScore:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
                        break;
                    case UIData.Property.drawScore:
                        {
                            ValueChangeUtils.replaceCallBack(this, syncs);
                            dirty = true;
                        }
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
            // Child
            {
                // editCalculateScoreWinLoseDraw
                {
                    if (wrapProperty.p is EditData<CalculateScoreWinLoseDraw>)
                    {
                        switch ((EditData<CalculateScoreWinLoseDraw>.Property)wrapProperty.n)
                        {
                            case EditData<CalculateScoreWinLoseDraw>.Property.origin:
                                dirty = true;
                                break;
                            case EditData<CalculateScoreWinLoseDraw>.Property.show:
                                {
                                    ValueChangeUtils.replaceCallBack(this, syncs);
                                    dirty = true;
                                }
                                break;
                            case EditData<CalculateScoreWinLoseDraw>.Property.compare:
                                {
                                    ValueChangeUtils.replaceCallBack(this, syncs);
                                    dirty = true;
                                }
                                break;
                            case EditData<CalculateScoreWinLoseDraw>.Property.compareOtherType:
                                dirty = true;
                                break;
                            case EditData<CalculateScoreWinLoseDraw>.Property.canEdit:
                                dirty = true;
                                break;
                            case EditData<CalculateScoreWinLoseDraw>.Property.editType:
                                dirty = true;
                                break;
                            default:
                                Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                break;
                        }
                        return;
                    }
                    // Child
                    {
                        if (wrapProperty.p is CalculateScoreWinLoseDraw)
                        {
                            switch ((CalculateScoreWinLoseDraw.Property)wrapProperty.n)
                            {
                                case CalculateScoreWinLoseDraw.Property.winScore:
                                    dirty = true;
                                    break;
                                case CalculateScoreWinLoseDraw.Property.loseScore:
                                    dirty = true;
                                    break;
                                case CalculateScoreWinLoseDraw.Property.drawScore:
                                    dirty = true;
                                    break;
                                default:
                                    Debug.LogError("Don't process: " + wrapProperty + "; " + this);
                                    break;
                            }
                            return;
                        }
                        // Parent
                        {
                            if (wrapProperty.p is Server)
                            {
                                Server.State.OnUpdateSyncStateChange(wrapProperty, this);
                                return;
                            }
                        }
                    }
                }
                // winScore, loseScore, drawScore
                if (wrapProperty.p is RequestChangeFloatUI.UIData)
                {
                    return;
                }
            }
            Debug.LogError("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
        }

        #endregion

    }
}