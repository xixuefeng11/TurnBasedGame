﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections;
using System.Collections.Generic;

namespace Makruk
{
	public class MakrukMoveUI : UIBehavior<MakrukMoveUI.UIData>
	{

		#region UIData

		public class UIData : LastMoveSub
		{

			public VP<ReferenceData<MakrukMove>> makrukMove;

			public VP<bool> isHint;

			#region Constructor

			public enum Property
			{
				makrukMove,
				isHint
			}

			public UIData() : base()
			{
				this.makrukMove = new VP<ReferenceData<MakrukMove>>(this, (byte)Property.makrukMove, new ReferenceData<MakrukMove>(null));
				this.isHint = new VP<bool>(this, (byte)Property.isHint, false);
			}

			#endregion

			public override GameMove.Type getType ()
			{
				return GameMove.Type.MakrukMove;
			}

		}

        #endregion

        public override int getStartAllocate()
        {
            return Setting.get().defaultChosenGame.v.getGame() == GameType.Type.Makruk ? 1 : 0;
        }

        #region Refresh

        private static Vector2 Delta = new Vector2 (4f, 4f);

		public Color normalColor = new Color (16/256f, 78/256f, 163/256f, 256/256f);
		public Color hintColor = Color.green;// new Color (0 / 256f, 1, 0, 256 / 256f);

		public Image imgPromotion;

		public override void refresh ()
		{
			if (dirty) {
				dirty = false;
				if (this.data != null) {
					MakrukMove makrukMove = this.data.makrukMove.v.data;
					if (makrukMove != null) {
						// set line
						{
							UILineRenderer lineRenderer = GetComponent<UILineRenderer> ();
							if (lineRenderer != null) {
								// line
								{
									// Find position;
									int fromX = 0;
									int fromY = 0;
									int destX = 0;
									int destY = 0;
									{
										MakrukMove.GetClickPosition (makrukMove.move.v, out fromX, out fromY, out destX, out destY);
									}
									// Make point array
									Vector2[] points;
									{
										List<Vector2> temp = new List<Vector2> ();
										// From
										{
											Vector2 fro = new Vector2 (fromX + 0.5f - Delta.x, 
												              fromY + 0.5f - Delta.y);
											temp.Add (fro);
										}
										// Middle: for horse move
										{
											// Check is horse move
											bool isHorseMove = false;
											{
												if ((Mathf.Abs (fromX - destX) == 2 && Mathf.Abs (fromY - destY) == 1)
												    || (Mathf.Abs (fromX - destX) == 1 && Mathf.Abs (fromY - destY) == 2)) {
													isHorseMove = true;
												}
											}
											// Make point
											if (isHorseMove) {
												Vector2 middle = Vector2.zero;
												//
												if (fromX + 2 == destX) {
													middle = new Vector2 (fromX + 0.5f + 1, fromY + 0.5f);
												} 
												//
												else if (fromX - 2 == destX) {
													middle = new Vector2 (fromX + 0.5f - 1, fromY + 0.5f);
												} 
												//
												else if (fromY + 2 == destY) {
													middle = new Vector2 (fromX + 0.5f, fromY + 1 + 0.5f);
												}
												//
												else if (fromY - 2 == destY) {
													middle = new Vector2 (fromX + 0.5f, fromY - 1 + 0.5f);
												}
												temp.Add (middle - Delta);
											} else {
												// Debug.Log ("not horse move");
											}
										}
										// Des
										{
											Vector2 des = new Vector2 (destX + 0.5f - Delta.x, destY + 0.5f - Delta.y);
											temp.Add (des);
										}
										// Set
										{
											points = new Vector2[temp.Count];
											for (int i = 0; i < temp.Count; i++) {
												points [i] = temp [i];
											}
										}
									}
									lineRenderer.Points = points;
								}
								// color
								{
									if (this.data.isHint.v) {
										lineRenderer.color = hintColor;
									} else {
										lineRenderer.color = normalColor;
									}
								}
							} else {
								Debug.LogError ("lineRenderer null: " + this);
							}
						}
						// imgPromotion
						if (imgPromotion != null) {
                            Setting.Style style = Setting.get().style.v;
							if (this.data.isHint.v) {
								MakrukMove.Move move = new MakrukMove.Move (makrukMove.move.v);
								if (move.type == Common.MoveType.PROMOTION) {
									// sprite
									{
										// get playerIndex
										int playerIndex = 0;
										{
											MakrukGameDataUI.UIData makrukGameDataUIData = this.data.findDataInParent<MakrukGameDataUI.UIData> ();
											if (makrukGameDataUIData != null) {
												GameData gameData = makrukGameDataUIData.gameData.v.data;
												if (gameData != null) {
													if (gameData.gameType.v != null && gameData.gameType.v is Makruk) {
														Makruk makruk = gameData.gameType.v as Makruk;
														playerIndex = makruk.getPlayerIndex ();
													} else {
														Debug.LogError ("makruk null: " + this);
													}
												} else {
													Debug.LogError ("gameData null: " + this);
												}
											} else {
												Debug.LogError ("useRuleInputUIData null: " + this);
											}
										}
										// process
										imgPromotion.sprite = MakrukSpriteContainer.get ().getSprite (style,
											Common.make_piece (playerIndex == 0 ? Common.Color.WHITE 
												: Common.Color.BLACK, move.promotion));
									}
									// position
									{
										int fromX = 0;
										int fromY = 0;
										int destX = 0;
										int destY = 0;
										MakrukMove.GetClickPosition (makrukMove.move.v, out fromX, out fromY, out destX, out destY);
										imgPromotion.transform.localPosition = new Vector3 (destX - 3.5f, destY - 3.5f, 0);
									}
								} else {
									imgPromotion.sprite = MakrukSpriteContainer.get ().getSprite (style, Common.Piece.NO_PIECE);
								}
							} else {
								Debug.Log ("not hint: " + this);
								imgPromotion.sprite = MakrukSpriteContainer.get ().getSprite (style, Common.Piece.NO_PIECE);
							}
						} else {
							Debug.LogError ("imgPromotion null: " + this);
						}
					} else {
						Debug.LogError ("makrukMove null: " + this);
					}
				} else {
					// Debug.LogError ("data null: " + this);
				}
			}
		}

		public override bool isShouldDisableUpdate ()
		{
			return true;
		}

		#endregion

		#region implement callBacks

		private MakrukGameDataUI.UIData parentMakrukGameDataUIData = null;

		public override void onAddCallBack<T> (T data)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
                // Setting
                Setting.get().addCallBack(this);
				// Parent
				{
					DataUtils.addParentCallBack (uiData, this, ref this.parentMakrukGameDataUIData);
				}
				// Child
				{
					uiData.makrukMove.allAddCallBack (this);
				}
				dirty = true;
				return;
			}
            // Setting
            if(data is Setting)
            {
                dirty = true;
                return;
            }
            // Parent
            {
				if (data is MakrukGameDataUI.UIData) {
					MakrukGameDataUI.UIData makrukGameDataUIData = data as MakrukGameDataUI.UIData;
					// Child
					{
						makrukGameDataUIData.gameData.allAddCallBack (this);
					}
					dirty = true;
					return;
				}
				// GameData
				{
					if (data is GameData) {
						GameData gameData = data as GameData;
						// Child
						{
							gameData.gameType.allAddCallBack (this);
						}
						dirty = true;
						return;
					}
					if (data is GameType) {
						dirty = true;
						return;
					}
				}
			}
			// Child
			if (data is MakrukMove) {
				dirty = true;
				return;
			}
			Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onRemoveCallBack<T> (T data, bool isHide)
		{
			if (data is UIData) {
				UIData uiData = data as UIData;
                // Setting
                Setting.get().removeCallBack(this);
				// Parent
				{
					DataUtils.removeParentCallBack (uiData, this, ref this.parentMakrukGameDataUIData);
				}
				// Child
				{
					uiData.makrukMove.allRemoveCallBack (this);
				}
				this.setDataNull (uiData);
				return;
			}
            // Setting
            if(data is Setting)
            {
                return;
            }
            // Parent
            {
				if (data is MakrukGameDataUI.UIData) {
					MakrukGameDataUI.UIData makrukGameDataUIData = data as MakrukGameDataUI.UIData;
					// Child
					{
						makrukGameDataUIData.gameData.allRemoveCallBack (this);
					}
					return;
				}
				// GameData
				{
					if (data is GameData) {
						GameData gameData = data as GameData;
						// Child
						{
							gameData.gameType.allRemoveCallBack (this);
						}
						return;
					}
					if (data is GameType) {
						return;
					}
				}
			}
			// Child
			if (data is MakrukMove) {
				return;
			}
			Debug.LogError ("Don't process: " + data + "; " + this);
		}

		public override void onUpdateSync<T> (WrapProperty wrapProperty, List<Sync<T>> syncs)
		{
			if (WrapProperty.checkError (wrapProperty)) {
				return;
			}
			if (wrapProperty.p is UIData) {
				switch ((UIData.Property)wrapProperty.n) {
				case UIData.Property.makrukMove:
					{
						ValueChangeUtils.replaceCallBack (this, syncs);
						dirty = true;
					}
					break;
				case UIData.Property.isHint:
					dirty = true;
					break;
				default:
					Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
            // Setting
            if(wrapProperty.p is Setting)
            {
                switch ((Setting.Property)wrapProperty.n)
                {
                    case Setting.Property.language:
                        break;
                    case Setting.Property.style:
                        dirty = true;
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
            // Parent
            {
				if (wrapProperty.p is MakrukGameDataUI.UIData) {
					switch ((MakrukGameDataUI.UIData.Property)wrapProperty.n) {
					case MakrukGameDataUI.UIData.Property.gameData:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case MakrukGameDataUI.UIData.Property.isOnAnimation:
						break;
					case MakrukGameDataUI.UIData.Property.board:
						break;
					case MakrukGameDataUI.UIData.Property.lastMove:
						break;
					case MakrukGameDataUI.UIData.Property.showHint:
						break;
					case MakrukGameDataUI.UIData.Property.inputUI:
						break;
					default:
						Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				if (wrapProperty.p is GameData) {
					switch ((GameData.Property)wrapProperty.n) {
					case GameData.Property.gameType:
						{
							ValueChangeUtils.replaceCallBack (this, syncs);
							dirty = true;
						}
						break;
					case GameData.Property.useRule:
						break;
					case GameData.Property.turn:
						break;
					case GameData.Property.timeControl:
						break;
					case GameData.Property.lastMove:
						break;
					case GameData.Property.state:
						break;
					default:
						Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
						break;
					}
					return;
				}
				if (wrapProperty.p is GameType) {
					if (wrapProperty.p is Makruk) {
						switch ((Makruk.Property)wrapProperty.n) {
						case Makruk.Property.board:
							break;
						case Makruk.Property.byTypeBB:
							break;
						case Makruk.Property.byColorBB:
							break;
						case Makruk.Property.pieceCount:
							break;
						case Makruk.Property.pieceList:
							break;
						case Makruk.Property.index:
							break;
						case Makruk.Property.gamePly:
							break;
						case Makruk.Property.sideToMove:
							dirty = true;
							break;
						case Makruk.Property.st:
							break;
						case Makruk.Property.chess960:
							break;
						default:
							Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
							break;
						}
						return;
					}
					return;
				}
			}
			// Child
			if (wrapProperty.p is MakrukMove) {
				switch ((MakrukMove.Property)wrapProperty.n) {
				case MakrukMove.Property.move:
					dirty = true;
					break;
				default:
					Debug.LogError ("Don't process: " + wrapProperty + "; " + this);
					break;
				}
				return;
			}
			Debug.LogError ("Don't process: " + wrapProperty + "; " + syncs + "; " + this);
		}

		#endregion
	}
}