#property copyright "www.broccoli-trade.ru"
#property link      "http://www.broccoli-trade.ru"


extern int Magic = 2012345;
extern string EA_Comment = "GarantedProfit";
extern bool Signal_1 = TRUE;
extern bool Signal_2 = TRUE;
extern bool Signal_3 = TRUE;
extern bool Signal_4 = TRUE;
extern double MaxSpread = 3.5;
extern int Slippage = 0;
extern bool NFA = FALSE;
bool gi_120 = FALSE;
extern bool AutoGMT_Offset = false;
extern int ManualGMT_Offset = 2;
extern bool DST_Usage = TRUE;
extern string CS = "==== Custom Settings ====";
bool gi_144 = TRUE;
extern int BetterPricePips = 0;
extern int MaxNegAdds = 3;
extern int ForceProfit = 0;
extern int ForceLoss = 0;
extern int FixedTakeProfit = 0;
extern int FixedStopLoss = 0;
bool gi_172 = FALSE;
string gs_gbpusd_176 = "GBPUSD";
extern string MM = "==== Risk Management ====";
extern bool RecoveryMode = FALSE;
extern double FixedLots = 0.1;
extern double AutoMM = 0.0;
extern double AutoMM_Max = 5.0;

//=============== 
// v3.x dll vars
//===============
int _BETTERPRICEPIPS;
int _MAXNEGADDS;
int _FORCEPROFIT;
int _FORCELOSS;
int _FIXEDTAKEPROFIT;
int _FIXEDSTOPLOSS;
int _BANDPER;
int _BANDPER_OPEN;
int _BREAKENTRY;
int _MAXBREAKENTRY;
int _BREAKEXIT;
int _TREND_PER;
int _TRENDDIST;
int _TREND_PER_2;
int _TRENDDIST_2;
int _EXITTIME;
int _EXITTIMEPROFIT;
int _EXITTIMEFORCEPROFIT;
int _MINBANDDIST;
int _RESCUEPROFIT;
int _NOTRENDHOUR1;
int _NOTRENDHOUR2;
int _NOTRENDHOUR3;
int _NOTRENDHOUR4;
int _FRIDAYENDHOUR;

//===============
// +v4 dll vars
//===============
int _BreakEntryH4Reverse;
int _BreakbandH4;
int _BandDistH4;
int _BandPer_H4;
int _MinBandDistH4;
int _BreakEntrySwingReverse;
int _SWING_MA_PER;
int _SWING_MA_DIST;
int _SWING_IMPULSE_MA_PER;
int _SWING_MA_IMPULSE;
int _BandPer_Open_Swing;
//==============

int gi_220 = 100;
int g_period_224 = 0;
int g_period_228 = 0;
int gi_232 = 0;
int gi_236 = 0;
int gi_240 = 0;
int g_period_244 = 0;
int gi_248 = 0;
int g_period_252 = 0;
int gi_256 = 0;
int gi_260 = 0;
int gi_264 = 0;
int gi_268 = 0;
int gi_272 = -1;
int gi_276 = 0;
int gi_280 = 0;
int gi_284 = 0;
int g_period_288 = 0;
int gi_292 = 0;
int gi_296 = 0;
int g_period_300 = 0;
int gi_304 = 0;
int g_period_308 = 0;
int gi_312 = 0;
int g_period_316 = 0;
int gi_320 = 100;
double gd_324 = 10.0;
double gd_332 = 1.05;
int gi_340 = 3;
int gi_344 = 3;
int gi_348 = 0;
int gi_352 = 0;
int gi_356 = -1;
string gs_360 = "";
int gi_368 = 2;
bool gi_372 = TRUE;
bool gi_376 = FALSE;
int g_stoplevel_380 = 0;
double g_minlot_384 = 0.01;
double g_maxlot_392 = 0.01;
double g_lotstep_400 = 0.01;
int g_lotsize_408 = 100000;
double g_marginrequired_412 = 1000.0;
double gd_420 = 0.0001;
double gd_unused_428 = 0.1;
int g_datetime_436 = 0;
double gd_unused_440 = 1.0;
int account_number=15160;
void init() {
   gi_372 = TRUE;
   gi_356 = -1;
   Comment("");
   if (ObjectFind("BKGR") >= 0) ObjectDelete("BKGR");
   if (ObjectFind("BKGR2") >= 0) ObjectDelete("BKGR2");
   if (ObjectFind("BKGR3") >= 0) ObjectDelete("BKGR3");
   if (ObjectFind("BKGR4") >= 0) ObjectDelete("BKGR4");
   if (ObjectFind("LV") >= 0) ObjectDelete("LV");
}

int deinit() {
   Comment("");
   if (ObjectFind("BKGR") >= 0) ObjectDelete("BKGR");
   if (ObjectFind("BKGR2") >= 0) ObjectDelete("BKGR2");
   if (ObjectFind("BKGR3") >= 0) ObjectDelete("BKGR3");
   if (ObjectFind("BKGR4") >= 0) ObjectDelete("BKGR4");
   if (ObjectFind("LV") >= 0) ObjectDelete("LV");
   if (gi_356 != -1) gi_356 = f0_5();
   return (0);
}

int start() {
   double price_8;
   color color_32;
   int ticket_168;
   color color_220;
   string ls_224;
   string ls_368;
   string ls_0 = "";
   double lots_40 = 0;
   double ld_48 = 0;
   double ld_56 = 0;
   double ld_64 = 0;
   double ld_72 = 0;
   double ld_80 = 1;
   //�������� ������
   if (AccountNumber()!=account_number)
   {
   Alert("�������� ����... �������� �� ��������");
   return(0);
   }
   if (DayOfWeek() == 1 && iVolume(NULL, PERIOD_D1, 0) < 5.0) return (0);
   if (StringLen(Symbol()) < 6) return (0);
   if (gi_372) {
      Comment("\nInitializing ...");
      Sleep(1000);
      RefreshRates();
      gi_372 = FALSE;
      g_stoplevel_380 = MarketInfo(Symbol(), MODE_STOPLEVEL);
      g_minlot_384 = MarketInfo(Symbol(), MODE_MINLOT);
      g_maxlot_392 = MarketInfo(Symbol(), MODE_MAXLOT);
      g_lotsize_408 = MarketInfo(Symbol(), MODE_LOTSIZE);
      g_lotstep_400 = MarketInfo(Symbol(), MODE_LOTSTEP);
      g_marginrequired_412 = MarketInfo(Symbol(), MODE_MARGINREQUIRED);
      if (Digits <= 3) gd_420 = 0.01;
      else gd_420 = 0.0001;
      if (Digits == 3 || Digits == 5) gd_unused_428 = 0.1;
      else gd_unused_428 = 1;
      Sleep(1000);
      //gi_376 = f0_0();
      gi_376 = true;
      Sleep(1000);
      if (!gi_376) gi_372 = TRUE;
   }
   if ((!IsTesting()) && IsStopped()) return (0);
   if ((!IsTesting()) && !IsTradeAllowed()) return (0);
   if ((!IsTesting()) && IsTradeContextBusy()) return (0);
   if (IsDllsAllowed() == FALSE) {
      Comment("\nWarning: Set Parameter **AllowDLL Imports** ON in menu Tools -> Options -> ExpertAdvisors.");
      Print("Warning: Set Parameter **AllowDLL Imports** ON in menu Tools -> Options -> ExpertAdvisors.");
      Alert("Warning: Set Parameter **AllowDLL Imports** ON in menu Tools -> Options -> ExpertAdvisors.");
      Sleep(30000);
      return (0);
   }
   if (StringSubstr(Symbol(), 0, 6) != "GBPUSD" && StringSubstr(Symbol(), 0, 6) != "EURUSD") {
      Comment("\nUnsupported currency pair! Supported currency pairs: GBPUSD and EURUSD(Premium license only)");
      Alert("Unsupported currency pair! Supported currency pairs: GBPUSD and EURUSD(Premium license only)");
      Sleep(10000);
      return (0);
   }
   if (!IsTesting() && Period() != PERIOD_M15) {
      Comment("\nPlease, swich to M15 chart!");
      Alert("Please, swich to M15 chart!");
      Sleep(10000);
      return (0);
   }
   if (gi_356 <= 0) {
      if (!gi_376) {
         Comment("\nInternet connection problem");
         Alert("Internet connection problem");
         Sleep(10000);
         return (0);
      }
      gi_356 = f0_4();
      Sleep(1000);
      if (gi_356 < 0) Comment("\nInitializing ...");
   }
   if (gi_356 <= 0) {
      if (gi_356 == -1) {
         Comment("\nAccount login check is failed! Please, start GarantedProfit, after successful login in your MT4 account!");
         Alert("Account login check is failed! Please, start GarantedProfit, after successful login in your MT4 account!");
      } else {
         if (gi_356 == -2) {
            Comment("\nOnline authorization is failed! Please, register your account at http://www.broccali-trade.ru and restart your MT4 platform!");
            Alert("Online authorization is failed! Please, register your account at http://www.broccali-trade.ru and restart your MT4 platform!");
         } else {
            if (gi_356 == -7) {
               Comment("\nInaccurate local system time! Please, synchronize your computer system time and then restart your MT4 platform!");
               Alert("Inaccurate local system time! Please, synchronize your computer system time and then restart your MT4 platform!");
            } else {
               if (gi_356 == -8) {
                  Comment("\nUnsupported currency pair " + gs_360 + ", Bid: " + DoubleToStr(Bid, Digits) + ", Ask: " + DoubleToStr(Ask, Digits) + ". Supported currency pairs: GBPUSD and EURUSD(Premium license only)");
                  Alert("Unsupported currency pair " + gs_360 + ", Bid: " + DoubleToStr(Bid, Digits) + ", Ask: " + DoubleToStr(Ask, Digits) + ". Supported currency pairs: GBPUSD and EURUSD(Premium license only)");
               } else {
                  if (gi_356 == -9) {
                     Comment("\nLocal authorization check is failed! Please, restart MT4 platform!");
                     Alert("Local authorization check is failed! Please, restart MT4 platform!");
                  } else {
                     Comment("\nInitialization is failed with error code " + DoubleToStr(gi_356, 0));
                     Alert("Initialization is failed with error code " + DoubleToStr(gi_356, 0));
                  }
               }
            }
         }
      }
      Sleep(10000);
      return (0);
   }
   if (ForceProfit <= 0 || ForceLoss <= 0 || g_period_224 <= 0 || g_period_228 <= 0 || g_period_244 <= 0) {
      Comment("\nWrong initialization parameters for pair " + Symbol());
      Alert("Wrong initialization parameters for pair " + Symbol());
      Sleep(10000);
      return (0);
   }
   double order_open_price_88 = 0;
   double order_open_price_96 = 0;
   double ld_104 = 0;
   double ld_112 = 0;
   double ld_120 = 0;
   double ld_128 = 0;
   bool li_136 = FALSE;
   bool li_140 = FALSE;
   int li_144 = 0;
   int li_148 = 0;
   int count_152 = 0;
   int count_156 = 0;
   int count_160 = 0;
   int count_164 = 0;
   double ld_176 = 0;
   if (StringSubstr(AccountCurrency(), 0, 3) == "JPY") {
      ld_176 = MarketInfo("USDJPY" + StringSubstr(Symbol(), 6), MODE_BID);
      if (ld_176 > 0.1) ld_80 = ld_176;
      else ld_80 = 83;
   }
   if (StringSubstr(AccountCurrency(), 0, 3) == "GBP") {
      ld_176 = MarketInfo("GBPUSD" + StringSubstr(Symbol(), 6), MODE_BID);
      if (ld_176 > 0.1) ld_80 = 1 / ld_176;
      else ld_80 = 0.6329113924;
   }
   if (StringSubstr(AccountCurrency(), 0, 3) == "EUR") {
      ld_176 = MarketInfo("EURUSD" + StringSubstr(Symbol(), 6), MODE_BID);
      if (ld_176 > 0.1) ld_80 = 1 / ld_176;
      else ld_80 = 0.7575757576;
   }
   ld_48 = MathCeil(1.0 / ld_80 / 100.0 * AccountBalance() / g_lotstep_400 / (g_lotsize_408 / 100)) * g_lotstep_400;
   if (AutoMM > 0.0 && (!RecoveryMode)) lots_40 = MathMax(g_minlot_384, MathMin(g_maxlot_392, MathCeil(MathMin(AutoMM_Max, AutoMM) / ld_80 / 100.0 * AccountFreeMargin() / g_lotstep_400 / (g_lotsize_408 / 100)) * g_lotstep_400));
   if (AutoMM > 0.0 && RecoveryMode) lots_40 = f0_1();
   if (AutoMM == 0.0) lots_40 = FixedLots;
   double order_stoploss_184 = 0;
   double order_takeprofit_192 = 0;
   double order_stoploss_200 = 0;
   double order_takeprofit_208 = 0;
   for (int pos_216 = 0; pos_216 <= OrdersTotal() - 1; pos_216++) {
      if (!OrderSelect(pos_216, SELECT_BY_POS, MODE_TRADES)) Print("Error in OrderSelect! Position:", pos_216);
      else {
         if (OrderType() <= OP_SELL && OrderSymbol() == Symbol()) {
            if (OrderMagicNumber() != Magic) {
               if (OrderType() == OP_BUY) count_160++;
               else count_164++;
            }
            if (OrderMagicNumber() == Magic) {
               ld_72 += OrderProfit();
               if (OrderType() == OP_BUY) ld_56 = (Bid - OrderOpenPrice()) / gd_420;
               else ld_56 = (OrderOpenPrice() - Ask) / gd_420;
               ld_64 += ld_56;
               if (OrderType() == OP_BUY) {
                  count_152++;
                  ld_104 += OrderOpenPrice() * OrderLots();
                  ld_120 += OrderLots();
                  li_144 = MathMax(li_144, OrderOpenTime());
                  if (order_open_price_88 < Point || OrderOpenPrice() < order_open_price_88) order_open_price_88 = OrderOpenPrice();
                  if (!(FixedTakeProfit > 0 || FixedStopLoss > 0)) continue;
                  if (order_stoploss_184 < Point && order_takeprofit_192 < Point) {
                     order_stoploss_184 = OrderStopLoss();
                     order_takeprofit_192 = OrderTakeProfit();
                  }
                  if (order_stoploss_184 < Point && order_takeprofit_192 < Point) {
                     if (FixedStopLoss > 0) order_stoploss_184 = MathMin(OrderOpenPrice() - FixedStopLoss * gd_420, Bid - g_stoplevel_380 * Point);
                     if (FixedTakeProfit > 0) order_takeprofit_192 = MathMax(OrderOpenPrice() + FixedTakeProfit * gd_420, Ask + g_stoplevel_380 * Point);
                  }
                  if (!(OrderStopLoss() < Point && OrderTakeProfit() < Point)) continue;
                  if (!(MathAbs(order_stoploss_184 - OrderStopLoss()) >= Point || MathAbs(order_takeprofit_192 - OrderTakeProfit()) >= Point)) continue;
                  OrderModify(OrderTicket(), OrderOpenPrice(), order_stoploss_184, order_takeprofit_192, 0, CLR_NONE);
                  continue;
               }
               count_156++;
               ld_112 += OrderOpenPrice() * OrderLots();
               ld_128 += OrderLots();
               li_148 = MathMax(li_148, OrderOpenTime());
               if (order_open_price_96 < Point || OrderOpenPrice() > order_open_price_96) order_open_price_96 = OrderOpenPrice();
               if (FixedTakeProfit > 0 || FixedStopLoss > 0) {
                  if (order_stoploss_200 < Point && order_takeprofit_208 < Point) {
                     order_stoploss_200 = OrderStopLoss();
                     order_takeprofit_208 = OrderTakeProfit();
                  }
                  if (order_stoploss_200 < Point && order_takeprofit_208 < Point) {
                     if (FixedStopLoss > 0) order_stoploss_200 = MathMax(OrderOpenPrice() + FixedStopLoss * gd_420, Ask + g_stoplevel_380 * Point);
                     if (FixedTakeProfit > 0) order_takeprofit_208 = MathMin(OrderOpenPrice() - FixedTakeProfit * gd_420, Bid - g_stoplevel_380 * Point);
                  }
                  if (OrderStopLoss() < Point && OrderTakeProfit() < Point)
                     if (MathAbs(order_stoploss_200 - OrderStopLoss()) >= Point || MathAbs(order_takeprofit_208 - OrderTakeProfit()) >= Point) OrderModify(OrderTicket(), OrderOpenPrice(), order_stoploss_200, order_takeprofit_208, 0, CLR_NONE);
               }
            }
         }
      }
   }
   if (count_152 > 0 && ld_120 > 0.0) ld_104 = NormalizeDouble(ld_104 / ld_120, Digits);
   if (count_156 > 0 && ld_128 > 0.0) ld_112 = NormalizeDouble(ld_112 / ld_128, Digits);
   if ((!IsTesting()) || IsVisualMode()) {
      if ((!IsTesting() || TimeCurrent() >= g_datetime_436 + 1) || !gi_144 || iVolume(NULL, PERIOD_M1, 0) <= 1.0) {
         g_datetime_436 = TimeCurrent();
         color_220 = SeaGreen;
         if (lots_40 > 3.0 * ld_48) color_220 = Red;
         else
            if (lots_40 > 1.5 * ld_48) color_220 = OrangeRed;
         if (ObjectFind("BKGR") < 0) {
            ObjectCreate("BKGR", OBJ_LABEL, 0, 0, 0);
            ObjectSetText("BKGR", "g", 110, "Webdings", DarkSlateGray);
            ObjectSet("BKGR", OBJPROP_CORNER, 0);
            ObjectSet("BKGR", OBJPROP_BACK, TRUE);
            ObjectSet("BKGR", OBJPROP_XDISTANCE, 5);
            ObjectSet("BKGR", OBJPROP_YDISTANCE, 15);
         }
         if (ObjectFind("BKGR2") < 0) {
            ObjectCreate("BKGR2", OBJ_LABEL, 0, 0, 0);
            ObjectSetText("BKGR2", "g", 110, "Webdings", color_220);
            ObjectSet("BKGR2", OBJPROP_BACK, TRUE);
            ObjectSet("BKGR2", OBJPROP_XDISTANCE, 5);
            ObjectSet("BKGR2", OBJPROP_YDISTANCE, 60);
         } else ObjectSet("BKGR2", OBJPROP_COLOR, color_220);
         if (ObjectFind("BKGR3") < 0) {
            ObjectCreate("BKGR3", OBJ_LABEL, 0, 0, 0);
            ObjectSetText("BKGR3", "g", 110, "Webdings", color_220);
            ObjectSet("BKGR3", OBJPROP_CORNER, 0);
            ObjectSet("BKGR3", OBJPROP_BACK, TRUE);
            ObjectSet("BKGR3", OBJPROP_XDISTANCE, 5);
            ObjectSet("BKGR3", OBJPROP_YDISTANCE, 45);
         } else ObjectSet("BKGR3", OBJPROP_COLOR, color_220);
         if (ObjectFind("LV") < 0) {
            ObjectCreate("LV", OBJ_LABEL, 0, 0, 0);
            ObjectSetText("LV", "GARANTED PROFIT", 9, "Tahoma Bold", White);
            ObjectSet("LV", OBJPROP_CORNER, 0);
            ObjectSet("LV", OBJPROP_BACK, FALSE);
            ObjectSet("LV", OBJPROP_XDISTANCE, 13);
            ObjectSet("LV", OBJPROP_YDISTANCE, 23);
         }
         if (ObjectFind("BKGR4") < 0) {
            ObjectCreate("BKGR4", OBJ_LABEL, 0, 0, 0);
            ObjectSetText("BKGR4", "g", 110, "Webdings", color_220);
            ObjectSet("BKGR4", OBJPROP_CORNER, 0);
            ObjectSet("BKGR4", OBJPROP_BACK, TRUE);
            ObjectSet("BKGR4", OBJPROP_XDISTANCE, 5);
            if (AutoMM > 0.0) ObjectSet("BKGR4", OBJPROP_YDISTANCE, 155);
            else ObjectSet("BKGR4", OBJPROP_YDISTANCE, 132);
         } else ObjectSet("BKGR4", OBJPROP_COLOR, color_220);
         if (IsTesting()) DST_Usage = FALSE;
         if (DST_Usage == TRUE) ls_224 = "YES";
         if (DST_Usage == FALSE) ls_224 = "NO";
         ls_0 = ls_0 
            + "\n  " 
            + "\n " 
            + "\n  Authorization - OK!" 
            + "\n -----------------------------------------------" 
            + "\n  GMT : " + TimeToStr(TimeCurrent() - 3600 * gi_368, TIME_DATE|TIME_MINUTES|TIME_SECONDS) 
            + "\n  Broker : " + TimeToStr(TimeCurrent(), TIME_DATE|TIME_MINUTES|TIME_SECONDS) 
            + "\n  Broker GMT Offset: " + GMTOffset() 
            + "\n  DST_Usage: " + ls_224 
            + "\n -----------------------------------------------" 
            + "\n  Force SL / TP = " + ForceLoss + " / " + ForceProfit + " pips" 
            + "\n  Fixed SL / TP = " + FixedStopLoss + " / " + FixedTakeProfit + " pips" 
         + "\n  Spread = " + DoubleToStr((Ask - Bid) / gd_420, 1) + " pips";
         if (Ask - Bid > MaxSpread * gd_420) ls_0 = ls_0 + " - TOO HIGH";
         else ls_0 = ls_0 + " - NORMAL";
         ls_0 = ls_0 
         + "\n -----------------------------------------------";
         if (AutoMM > 0.0) {
            ls_0 = ls_0 
               + "\n  AutoMM - ENABLED" 
            + "\n  Risk = " + DoubleToStr(AutoMM, 1) + "%";
         }
         ls_0 = ls_0 
         + "\n  Lots = " + DoubleToStr(lots_40, 2);
         if (lots_40 <= 1.5 * ld_48) ls_0 = ls_0 + " - LOW RISK";
         if (lots_40 > 1.5 * ld_48 && lots_40 <= 3.0 * ld_48) ls_0 = ls_0 + " - MODERATE RISK";
         if (lots_40 > 3.0 * ld_48) ls_0 = ls_0 + " - HIGH RISK !!!";
         ls_0 = ls_0 
         + "\n -----------------------------------------------";
         if (RecoveryMode) {
            ls_0 = ls_0 
            + "\n  Recovery Mode - ENABLED";
         } else {
            ls_0 = ls_0 
            + "\n  Recovery Mode - DISABLED";
         }
         ls_0 = ls_0 
         + "\n -----------------------------------------------";
         ls_0 = ls_0 
         + "\n  Account Ballance = " + DoubleToStr(AccountBalance(), 2);
         if (count_152 == 0 && count_156 == 0) {
            ls_0 = ls_0 
               + "\n  No active trades" 
            + "\n";
         } else {
            if (count_152 > 0) {
               ls_0 = ls_0 
               + "\n  Buy average price " + DoubleToStr(ld_104, Digits);
            }
            if (count_156 > 0) {
               ls_0 = ls_0 
               + "\n  Sell average price " + DoubleToStr(ld_112, Digits);
            }
            ls_0 = ls_0 
               + "\n  Current trade " + DoubleToStr(ld_64, 1) + " pips" 
            + "\n  Account Profit = " + DoubleToStr(ld_72, 2);
         }
         Comment(ls_0);
      }
   }
   if (IsTesting() && StringSubstr(Symbol(), 0, 6) == "EURUSD" && Year() < 2008) return (0);
   if (IsTesting() && StringSubstr(Symbol(), 0, 6) == "GBPUSD" && Year() < 2008 || (Year() == 2008 && Month() < 5)) return (0);
   if (gi_144 && iVolume(NULL, PERIOD_M1, 0) > 1.0) return (0);
   HideTestIndicators(TRUE);
   double iopen_232 = iOpen(NULL, PERIOD_M1, 1);
   double iclose_240 = iClose(NULL, PERIOD_M1, 1);
   double ibands_248 = iBands(NULL, PERIOD_M15, g_period_224, 2, 0, PRICE_CLOSE, MODE_UPPER, 1);
   double ibands_256 = iBands(NULL, PERIOD_M15, g_period_224, 2, 0, PRICE_CLOSE, MODE_LOWER, 1);
   double ibands_264 = iBands(NULL, PERIOD_M15, g_period_228, 2, 0, PRICE_CLOSE, MODE_UPPER, 1);
   double ibands_272 = iBands(NULL, PERIOD_M15, g_period_228, 2, 0, PRICE_CLOSE, MODE_LOWER, 1);
   double ima_280 = iMA(NULL, PERIOD_H4, g_period_244, 0, MODE_SMMA, PRICE_CLOSE, 1);
   double ima_288 = iMA(NULL, PERIOD_H4, g_period_252, 0, MODE_SMMA, PRICE_CLOSE, 1);
   double iatr_296 = iATR(NULL, PERIOD_M15, g_period_224, 1);
   double irsi_304 = iRSI(NULL, PERIOD_M15, 0, g_period_224, 1);
   double iatr_312 = iATR(NULL, PERIOD_H4, g_period_244, 1);
   double irsi_320 = iRSI(NULL, PERIOD_H4, 0, g_period_244, 1);
   double ibands_328 = iBands(NULL, PERIOD_H4, g_period_288, 2, 0, PRICE_CLOSE, MODE_UPPER, 1);
   double ibands_336 = iBands(NULL, PERIOD_H4, g_period_288, 2, 0, PRICE_CLOSE, MODE_LOWER, 1);
   double ibands_344 = iBands(NULL, PERIOD_M15, g_period_316, 2, 0, PRICE_CLOSE, MODE_UPPER, 1);
   double ibands_352 = iBands(NULL, PERIOD_M15, g_period_316, 2, 0, PRICE_CLOSE, MODE_LOWER, 1);
   HideTestIndicators(FALSE);
   if (count_152 > 0) {
      if (CheckClose(gi_356, iclose_240, ibands_248 + gi_240 * gd_420, ibands_256 - gi_240 * gd_420, Bid, f0_2(), iatr_296, irsi_304) == 0) li_136 = TRUE;
      if ((Bid >= ld_104 + ForceProfit * gd_420 && CheckExitTimeFP(gi_356, TimeCurrent() - li_144, iatr_312, irsi_320)) || Bid <= ld_104 - ForceLoss * gd_420) li_136 = TRUE;
      if (Bid >= ld_104 + gi_260 * gd_420 && CheckExitTime(gi_356, TimeCurrent() - li_144, iatr_312, irsi_320)) li_136 = TRUE;
      if (Bid >= ld_104 + gi_268 * gd_420 && iopen_232 > iclose_240) li_136 = TRUE;
   }
   if (count_156 > 0) {
      if (CheckClose(gi_356, iclose_240, ibands_248 + gi_240 * gd_420, ibands_256 - gi_240 * gd_420, Bid, f0_2(), iatr_296, irsi_304) == 1) li_140 = TRUE;
      if ((Ask <= ld_112 - ForceProfit * gd_420 && CheckExitTimeFP(gi_356, TimeCurrent() - li_148, iatr_312, irsi_320)) || Ask >= ld_112 + ForceLoss * gd_420) li_140 = TRUE;
      if (Ask <= ld_112 - gi_260 * gd_420 && CheckExitTime(gi_356, TimeCurrent() - li_148, iatr_312, irsi_320)) li_140 = TRUE;
      if (Ask <= ld_112 - gi_268 * gd_420 && iopen_232 < iclose_240) li_140 = TRUE;
   }
   if (li_136 || li_140) {
      for (pos_216 = 0; pos_216 <= OrdersTotal() - 1; pos_216++) {
         if (!OrderSelect(pos_216, SELECT_BY_POS, MODE_TRADES)) Print("Error in OrderSelect! Position:", pos_216);
         else {
            if (OrderType() <= OP_SELL && OrderSymbol() == Symbol() && OrderMagicNumber() == Magic) {
               if (OrderType() == OP_BUY && li_136) {
                  for (int li_172 = 1; li_172 <= MathMax(1, gi_340); li_172++) {
                     RefreshRates();
                     if (OrderClose(OrderTicket(), OrderLots(), NormalizeDouble(Bid, Digits), Slippage, Violet)) {
                        count_152--;
                        pos_216--;
                        break;
                     }
                     Sleep(MathMax(100, 1000 * gi_344));
                  }
                  Sleep(5000);
                  continue;
               }
               if (OrderType() == OP_SELL && li_140) {
                  for (li_172 = 1; li_172 <= MathMax(1, gi_340); li_172++) {
                     RefreshRates();
                     if (OrderClose(OrderTicket(), OrderLots(), NormalizeDouble(Ask, Digits), Slippage, Violet)) {
                        count_156--;
                        pos_216--;
                        break;
                     }
                     Sleep(MathMax(100, 1000 * gi_344));
                  }
                  Sleep(5000);
               }
            }
         }
      }
   }
   bool li_360 = TRUE;
   bool li_364 = TRUE;
   if (NFA == TRUE && count_156 > 0 || count_164 > 0 || count_160 > 0) li_360 = FALSE;
   if (NFA == TRUE && count_152 > 0 || count_160 > 0 || count_164 > 0) li_364 = FALSE;
   if (gi_120 == TRUE && count_156 > 0 || count_164 > 0) li_360 = FALSE;
   if (gi_120 == TRUE && count_152 > 0 || count_160 > 0) li_364 = FALSE;
   if (OrdersTotal() >= gi_220) return (0);
   if (DayOfWeek() == 5 && Hour() >= gi_368 && f0_2() >= gi_272) return (0);
   if (ibands_264 - ibands_272 < gi_264 * gd_420) return (0);
   int cmd_36 = -1;
   if (count_152 <= MaxNegAdds) {
      if ((count_152 < 1 && Signal_1 == TRUE && CheckSignal1(gi_356, Bid, ibands_264 + gi_232 * gd_420, ibands_264 + gi_236 * gd_420, ibands_272 - gi_232 * gd_420, ibands_272 - gi_236 * gd_420,
         MathMin(ima_280 + gi_248 * gd_420, ima_288 + gi_256 * gd_420), MathMax(ima_280 - gi_248 * gd_420, ima_288 - gi_256 * gd_420)) == 0) || (count_152 < 1 && Signal_2 == TRUE &&
         CheckSignal2(gi_356, Bid, ibands_264 + gi_232 * gd_420, ibands_264 + gi_236 * gd_420, ibands_272 - gi_232 * gd_420, ibands_272 - gi_236 * gd_420, f0_2()) == 0) || (count_152 < 1 && Signal_3 == TRUE && CheckSignal3(gi_356,
         Bid, MathMax(ibands_264 + gi_276 * gd_420, ibands_328 - gi_284 * gd_420), MathMin(ibands_272 - gi_276 * gd_420, ibands_336 + gi_284 * gd_420), iHigh(NULL, PERIOD_H4,
         1), iLow(NULL, PERIOD_H4, 1), ibands_328 + gi_280 * gd_420, ibands_336 - gi_280 * gd_420, ibands_328 - ibands_336, gi_292 * gd_420) == 0) || (count_152 < 1 && Signal_4 == TRUE &&
         CheckSignal4(gi_356, Bid, MathMax(ibands_344 + gi_296 * gd_420, iMA(NULL, PERIOD_H1, g_period_300, 0, MODE_SMA, PRICE_CLOSE, 1) + gi_304 * gd_420), iMA(NULL, PERIOD_H1,
         g_period_308, 0, MODE_SMMA, PRICE_CLOSE, 1) - gi_312 * gd_420, MathMin(ibands_352 - gi_296 * gd_420, iMA(NULL, PERIOD_H1, g_period_300, 0, MODE_SMA, PRICE_CLOSE,
         1) - gi_304 * gd_420), iMA(NULL, PERIOD_H1, g_period_308, 0, MODE_SMMA, PRICE_CLOSE, 1) + gi_312 * gd_420) == 0) || (count_152 >= 1 && Ask < order_open_price_88 - BetterPricePips * gd_420)) {
         if (Ask - Bid > MaxSpread * gd_420) {
            Print("BUY not taken!!! - High spead...");
            Sleep(1000);
         } else {
            if (!li_360) {
               Print("BUY not taken!!! - No Hedge, or FIFO restriction ...");
               Sleep(1000);
            } else {
               ls_368 = "BUY";
               cmd_36 = 0;
               color_32 = Aqua;
               RefreshRates();
               price_8 = NormalizeDouble(Ask, Digits);
            }
         }
      }
   }
   if (count_156 <= MaxNegAdds) {
      if ((count_156 < 1 && Signal_1 == TRUE && CheckSignal1(gi_356, Bid, ibands_264 + gi_232 * gd_420, ibands_264 + gi_236 * gd_420, ibands_272 - gi_232 * gd_420, ibands_272 - gi_236 * gd_420,
         MathMin(ima_280 + gi_248 * gd_420, ima_288 + gi_256 * gd_420), MathMax(ima_280 - gi_248 * gd_420, ima_288 - gi_256 * gd_420)) == 1) || (count_156 < 1 && Signal_2 == TRUE &&
         CheckSignal2(gi_356, Bid, ibands_264 + gi_232 * gd_420, ibands_264 + gi_236 * gd_420, ibands_272 - gi_232 * gd_420, ibands_272 - gi_236 * gd_420, f0_2()) == 1) || (count_156 < 1 && Signal_3 == TRUE && CheckSignal3(gi_356,
         Bid, MathMax(ibands_264 + gi_276 * gd_420, ibands_328 - gi_284 * gd_420), MathMin(ibands_272 - gi_276 * gd_420, ibands_336 + gi_284 * gd_420), iHigh(NULL, PERIOD_H4,
         1), iLow(NULL, PERIOD_H4, 1), ibands_328 + gi_280 * gd_420, ibands_336 - gi_280 * gd_420, ibands_328 - ibands_336, gi_292 * gd_420) == 1) || (count_156 < 1 && Signal_4 == TRUE &&
         CheckSignal4(gi_356, Bid, MathMax(ibands_344 + gi_296 * gd_420, iMA(NULL, PERIOD_H1, g_period_300, 0, MODE_SMA, PRICE_CLOSE, 1) + gi_304 * gd_420), iMA(NULL, PERIOD_H1,
         g_period_308, 0, MODE_SMMA, PRICE_CLOSE, 1) - gi_312 * gd_420, MathMin(ibands_352 - gi_296 * gd_420, iMA(NULL, PERIOD_H1, g_period_300, 0, MODE_SMA, PRICE_CLOSE,
         1) - gi_304 * gd_420), iMA(NULL, PERIOD_H1, g_period_308, 0, MODE_SMMA, PRICE_CLOSE, 1) + gi_312 * gd_420) == 1) || (count_156 >= 1 && Bid > order_open_price_96 +
         BetterPricePips * gd_420)) {
         if (Ask - Bid > MaxSpread * gd_420) {
            Print("SELL not taken!!! - High spead...");
            Sleep(1000);
         } else {
            if (!li_364) {
               Print("SELL not taken!!! - No Hedge, or FIFO restriction ...");
               Sleep(1000);
            } else {
               ls_368 = "SELL";
               cmd_36 = 1;
               color_32 = Red;
               RefreshRates();
               price_8 = NormalizeDouble(Bid, Digits);
            }
         }
      }
   }
   if (cmd_36 >= OP_BUY && f0_3()) {
      for (li_172 = 1; li_172 <= MathMax(1, gi_340); li_172++) {
         ticket_168 = OrderSend(Symbol(), cmd_36, lots_40, price_8, Slippage * (gd_420 / Point), 0, 0, EA_Comment, Magic, 0, color_32);
         if (ticket_168 >= 0) break;
         Sleep(MathMax(100, 1000 * gi_344));
         RefreshRates();
         if (cmd_36 == OP_BUY) price_8 = NormalizeDouble(Ask, Digits);
         else
            if (cmd_36 == OP_SELL) price_8 = NormalizeDouble(Bid, Digits);
      }
      Sleep(1000);
      if (ticket_168 > 0) {
         if (OrderSelect(ticket_168, SELECT_BY_TICKET, MODE_TRADES)) {
            Print("Order " + ls_368 + " opened!: ", OrderOpenPrice());
            if (FixedTakeProfit > 0 || FixedStopLoss > 0) {
               if (OrderType() == OP_BUY) {
                  if (count_152 == 0) {
                     order_stoploss_184 = 0;
                     order_takeprofit_192 = 0;
                     if (FixedStopLoss > 0) order_stoploss_184 = MathMin(OrderOpenPrice() - FixedStopLoss * gd_420, Bid - g_stoplevel_380 * Point);
                     if (FixedTakeProfit > 0) order_takeprofit_192 = MathMax(OrderOpenPrice() + FixedTakeProfit * gd_420, Ask + g_stoplevel_380 * Point);
                  }
                  if (MathAbs(order_stoploss_184 - OrderStopLoss()) >= Point || MathAbs(order_takeprofit_192 - OrderTakeProfit()) >= Point) OrderModify(OrderTicket(), OrderOpenPrice(), order_stoploss_184, order_takeprofit_192, 0, CLR_NONE);
               } else {
                  if (OrderType() == OP_SELL) {
                     if (count_156 == 0) {
                        order_stoploss_200 = 0;
                        order_takeprofit_208 = 0;
                        if (FixedStopLoss > 0) order_stoploss_200 = MathMax(OrderOpenPrice() + FixedStopLoss * gd_420, Ask + g_stoplevel_380 * Point);
                        if (FixedTakeProfit > 0) order_takeprofit_208 = MathMin(OrderOpenPrice() - FixedTakeProfit * gd_420, Bid - g_stoplevel_380 * Point);
                     }
                     if (MathAbs(order_stoploss_200 - OrderStopLoss()) >= Point || MathAbs(order_takeprofit_208 - OrderTakeProfit()) >= Point) OrderModify(OrderTicket(), OrderOpenPrice(), order_stoploss_200, order_takeprofit_208, 0, CLR_NONE);
                  }
               }
            }
         }
      } else Print("Error opening " + ls_368 + " order!: ", GetLastError());
      Sleep(4000);
   }
   return (0);
}

double f0_1() {
   double ld_16;
   int count_24;
   double ld_28;
   int li_36;
   double ld_40;
   int li_48;
   double ld_52;
   int li_60;
   double ld_8 = 1;
   if (gd_332 > 0.0 && AutoMM > 0.0) {
      ld_16 = 0;
      count_24 = 0;
      ld_28 = 0;
      li_36 = 0;
      ld_40 = 0;
      li_48 = 0;
      for (int pos_64 = OrdersHistoryTotal() - 1; pos_64 >= 0; pos_64--) {
         if (OrderSelect(pos_64, SELECT_BY_POS, MODE_HISTORY)) {
            if (OrderSymbol() == Symbol() && OrderMagicNumber() == Magic) {
               count_24++;
               ld_16 += OrderProfit();
               if (ld_16 > ld_40) {
                  ld_40 = ld_16;
                  li_48 = count_24;
               }
               if (ld_16 < ld_28) {
                  ld_28 = ld_16;
                  li_36 = count_24;
               }
               if (count_24 >= gi_320) break;
            }
         }
      }
      if (li_48 <= li_36) ld_8 = MathPow(gd_332, li_36);
      else {
         ld_16 = ld_40;
         count_24 = li_48;
         ld_52 = ld_40;
         li_60 = li_48;
         for (pos_64 = OrdersHistoryTotal() - li_48 - 1; pos_64 >= 0; pos_64--) {
            if (OrderSelect(pos_64, SELECT_BY_POS, MODE_HISTORY)) {
               if (OrderSymbol() == Symbol() && OrderMagicNumber() == Magic) {
                  if (count_24 >= gi_320) break;
                  count_24++;
                  ld_16 += OrderProfit();
                  if (ld_16 < ld_52) {
                     ld_52 = ld_16;
                     li_60 = count_24;
                  }
               }
            }
         }
         if (li_60 == li_48 || ld_52 == ld_40) ld_8 = MathPow(gd_332, li_36);
         else {
            if (MathAbs(ld_28 - ld_40) / MathAbs(ld_52 - ld_40) >= (gd_324 + 100.0) / 100.0) ld_8 = MathPow(gd_332, li_36);
            else ld_8 = MathPow(gd_332, li_60);
         }
      }
   }
   for (double ld_ret_0 = MathMax(g_minlot_384, MathMin(g_maxlot_392, MathCeil(MathMin(AutoMM_Max, ld_8 * AutoMM) / 100.0 * AccountFreeMargin() / g_lotstep_400 / (g_lotsize_408 / 100)) * g_lotstep_400)); ld_ret_0 >= 2.0 * g_minlot_384 &&
      1.05 * (ld_ret_0 * g_marginrequired_412) >= AccountFreeMargin(); ld_ret_0 -= g_minlot_384) {
   }
   return (ld_ret_0);
}

int f0_3() {
   int datetime_4;
   bool li_ret_0 = TRUE;
   if (gi_348 > 0 && gi_352 > 0) {
      datetime_4 = 0;
      for (int pos_8 = OrdersHistoryTotal() - 1; pos_8 >= 0; pos_8--) {
         if (OrderSelect(pos_8, SELECT_BY_POS, MODE_HISTORY)) {
            if (OrderSymbol() == Symbol() && OrderMagicNumber() == Magic) {
               if (!((OrderType() == OP_BUY && (OrderClosePrice() - OrderOpenPrice()) / gd_420 <= (-gi_348)) || (OrderType() == OP_SELL && (OrderOpenPrice() - OrderClosePrice()) / gd_420 <= (-gi_348)))) break;
               datetime_4 = OrderCloseTime();
               break;
            }
         }
      }
      if (TimeCurrent() - datetime_4 < 3600 * gi_352) li_ret_0 = FALSE;
   }
   return (li_ret_0);
}

int f0_2() {
   int li_ret_0 = Hour();
   li_ret_0 -= gi_368;
   if (IsTesting()) DST_Usage = FALSE;
   if (DST_Usage == TRUE && (Month() > 3 && Month() < 11)) li_ret_0++;
   if (li_ret_0 > 23) li_ret_0 -= 24;
   if (li_ret_0 < 0) li_ret_0 += 24;
   return (li_ret_0);
}

int f0_4() {
   if (gi_172) gs_360 = StringSubstr(gs_gbpusd_176, 0, 6);
   else gs_360 = StringSubstr(Symbol(), 0, 6);
   int li_ret_0 = SessionInit(AccountNumber(), IsTesting(), IsDemo(), WindowHandle(Symbol(), Period()), gs_360, TimeCurrent());
   if (li_ret_0 == -8 && StringFind(",GBPUSD,", "," + gs_360 + ",") >= 0) {
      Comment("\nUpdating settings (" + gs_360 + ")...");
      li_ret_0 = SessionDeinit(AccountNumber(), IsTesting(), IsDemo(), WindowHandle(Symbol(), Period()), gs_360);
      Sleep(3000);
      li_ret_0 = SessionInit(AccountNumber(), IsTesting(), IsDemo(), WindowHandle(Symbol(), Period()), gs_360, TimeCurrent());
   }
   if (li_ret_0 >= 0) {
      if (!IsTesting() && AutoGMT_Offset == TRUE) gi_368 = GMTOffset();
      else gi_368 = ManualGMT_Offset;
      if (BetterPricePips <= 0) BetterPricePips = ParamValue(li_ret_0, "BetterPricePips");
      if (ForceProfit <= 0) ForceProfit = ParamValue(li_ret_0, "ForceProfit");
      if (ForceLoss <= 0) ForceLoss = ParamValue(li_ret_0, "ForceLoss");
      if (FixedTakeProfit <= 0) FixedTakeProfit = ParamValue(li_ret_0, "FixedTakeProfit");
      if (FixedStopLoss <= 0) FixedStopLoss = ParamValue(li_ret_0, "FixedStopLoss");
      if (g_period_224 == 0) g_period_224 = ParamValue(li_ret_0, "BandPer");
      if (g_period_228 == 0) g_period_228 = ParamValue(li_ret_0, "BandPer_Open");
      if (gi_232 == 0) gi_232 = ParamValue(li_ret_0, "BreakEntry");
      if (gi_236 == 0) gi_236 = ParamValue(li_ret_0, "MaxBreakEntry");
      if (gi_240 == 0) gi_240 = ParamValue(li_ret_0, "BreakExit");
      if (g_period_244 == 0) g_period_244 = ParamValue(li_ret_0, "TREND_PER");
      if (gi_248 == 0) gi_248 = ParamValue(li_ret_0, "TrendDist");
      if (g_period_252 == 0) g_period_252 = ParamValue(li_ret_0, "TREND_PER_2");
      if (gi_256 == 0) gi_256 = ParamValue(li_ret_0, "TrendDist_2");
      if (gi_260 == 0) gi_260 = ParamValue(li_ret_0, "ExitTimeProfit");
      if (gi_264 == 0) gi_264 = ParamValue(li_ret_0, "MinBandDist");
      if (gi_268 == 0) gi_268 = ParamValue(li_ret_0, "RescueProfit");
      if (gi_272 < 0) gi_272 = ParamValue(li_ret_0, "FridayEndHour");
      if (gi_276 == 0) gi_276 = ParamValue(li_ret_0, "BreakEntryH4Reverse");
      if (gi_280 == 0) gi_280 = ParamValue(li_ret_0, "BreakbandH4");
      if (gi_284 == 0) gi_284 = ParamValue(li_ret_0, "BandDistH4");
      if (g_period_288 == 0) g_period_288 = ParamValue(li_ret_0, "BandPer_H4");
      if (gi_292 == 0) gi_292 = ParamValue(li_ret_0, "MinBandDistH4");
      if (gi_296 == 0) gi_296 = ParamValue(li_ret_0, "BreakEntrySwingReverse");
      if (g_period_300 == 0) g_period_300 = ParamValue(li_ret_0, "SWING_MA_PER");
      if (gi_304 == 0) gi_304 = ParamValue(li_ret_0, "SWING_MA_DIST");
      if (g_period_308 == 0) g_period_308 = ParamValue(li_ret_0, "SWING_IMPULSE_MA_PER");
      if (gi_312 == 0) gi_312 = ParamValue(li_ret_0, "SWING_MA_IMPULSE");
      if (g_period_316 == 0) g_period_316 = ParamValue(li_ret_0, "BandPer_Open_Swing");
   }
   return (li_ret_0);
}

int f0_5() {
   int li_ret_0;
   if (gi_172) li_ret_0 = SessionDeinit(AccountNumber(), IsTesting(), IsDemo(), WindowHandle(Symbol(), Period()), StringSubstr(gs_gbpusd_176, 0, 6));
   else li_ret_0 = SessionDeinit(AccountNumber(), IsTesting(), IsDemo(), WindowHandle(Symbol(), Period()), StringSubstr(Symbol(), 0, 6));
   return (li_ret_0);
}

// decomp
int SessionInit(int accnum, int istest, int isdem, int winhandle, string pairname, int currtime)
{
   return(1);
}

int ParamValue(int a0, string a1)
{
   string pairname = StringSubstr(Symbol(), 0, 6);
 
   if (pairname=="EURUSD")
   {
      _EXITTIME = 490;
      _EXITTIMEFORCEPROFIT = 0;
      _NOTRENDHOUR1 = 4;
      _NOTRENDHOUR2 = 18;
      _NOTRENDHOUR3 = 55;
      _NOTRENDHOUR4 = 55;
      _BETTERPRICEPIPS = 6;
      if (a1=="BetterPricePips") return (_BETTERPRICEPIPS);
      _FORCEPROFIT = 8; //9 v3
      if (a1=="ForceProfit") return (_FORCEPROFIT);
      _FORCELOSS = 300;
      if (a1=="ForceLoss") return (_FORCELOSS);
      _FIXEDTAKEPROFIT = 0;
      if (a1=="FixedTakeProfit") return (_FIXEDTAKEPROFIT);
      _FIXEDSTOPLOSS = 0;
      if (a1=="FixedStopLoss") return (_FIXEDSTOPLOSS);
      _BANDPER = 19;
      if (a1=="BandPer") return (_BANDPER);
      _BANDPER_OPEN = 15;
      if (a1=="BandPer_Open") return (_BANDPER_OPEN);
      _BREAKENTRY = 3;
      if (a1=="BreakEntry") return (_BREAKENTRY);
      _MAXBREAKENTRY = 8;
      if (a1=="MaxBreakEntry") return (_MAXBREAKENTRY);
      _BREAKEXIT = 3;
      if (a1=="BreakExit") return (_BREAKEXIT);
      _TREND_PER = 22;
      if (a1=="TREND_PER") return (_TREND_PER);
      _TRENDDIST = 100;
      if (a1=="TrendDist") return (_TRENDDIST);
      _TREND_PER_2 = 5;
      if (a1=="TREND_PER_2") return (_TREND_PER_2);
      _TRENDDIST_2 = 55;
      if (a1=="TrendDist_2") return (_TRENDDIST_2);
      _EXITTIMEPROFIT = -40;
      if (a1=="ExitTimeProfit") return (_EXITTIMEPROFIT);
      _MINBANDDIST = 8;
      if (a1=="MinBandDist") return (_MINBANDDIST);
      _RESCUEPROFIT = 7;
      if (a1=="RescueProfit") return (_RESCUEPROFIT);
      _FRIDAYENDHOUR = 21;
      if (a1=="FridayEndHour") return (_FRIDAYENDHOUR);
      //v4
      _BreakEntryH4Reverse = 7;
      if (a1=="BreakEntryH4Reverse") return (_BreakEntryH4Reverse);
      _BreakbandH4 = 28;
      if (a1=="BreakbandH4") return (_BreakbandH4);
      _BandDistH4 = 42;
      if (a1=="BandDistH4") return (_BandDistH4);
      _BandPer_H4 = 5;
      if (a1=="BandPer_H4") return (_BandPer_H4);
      _MinBandDistH4 = 5;
      if (a1=="MinBandDistH4") return (_MinBandDistH4);
      _BreakEntrySwingReverse = -1;
      if (a1=="BreakEntrySwingReverse") return (_BreakEntrySwingReverse);
      _SWING_MA_PER = 55;
      if (a1=="SWING_MA_PER") return (_SWING_MA_PER);
      _SWING_MA_DIST = 60;
      if (a1=="SWING_MA_DIST") return (_SWING_MA_DIST);
      _SWING_IMPULSE_MA_PER = 9;
      if (a1=="SWING_IMPULSE_MA_PER") return (_SWING_IMPULSE_MA_PER);
      _SWING_MA_IMPULSE = 0;
      if (a1=="SWING_MA_IMPULSE") return (_SWING_MA_IMPULSE);
      _BandPer_Open_Swing = 4;
      if (a1=="BandPer_Open_Swing") return (_BandPer_Open_Swing);
  }    
  if (pairname=="GBPUSD")
  {
      _EXITTIME=600;
      _EXITTIMEFORCEPROFIT=0;
      _NOTRENDHOUR1=22;
      _NOTRENDHOUR2=4; //15
      _NOTRENDHOUR3=15; //55
      _NOTRENDHOUR4=55;
      _BETTERPRICEPIPS = 15;
      if (a1=="BetterPricePips") return (_BETTERPRICEPIPS);
      _FORCEPROFIT = 20;
      if (a1=="ForceProfit") return (_FORCEPROFIT);
      _FORCELOSS = 380;
      if (a1=="ForceLoss") return (_FORCELOSS);
      _FIXEDTAKEPROFIT = 0;
      if (a1=="FixedTakeProfit") return (_FIXEDTAKEPROFIT);
      _FIXEDSTOPLOSS = 0;
      if (a1=="FixedStopLoss") return (_FIXEDSTOPLOSS);
      _BANDPER = 22;
      if (a1=="BandPer") return (_BANDPER);
      _BANDPER_OPEN = 10;
      if (a1=="BandPer_Open") return (_BANDPER_OPEN);
      _BREAKENTRY = 1;
      if (a1=="BreakEntry") return (_BREAKENTRY);
      _MAXBREAKENTRY = 80; //50  v3
      if (a1=="MaxBreakEntry") return (_MAXBREAKENTRY);
      _BREAKEXIT = 9;
      if (a1=="BreakExit") return (_BREAKEXIT);
      _TREND_PER = 21;
      if (a1=="TREND_PER") return (_TREND_PER);
      _TRENDDIST = 100;
      if (a1=="TrendDist") return (_TRENDDIST);
      _TREND_PER_2 = 9; //21 v3
      if (a1=="TREND_PER_2") return (_TREND_PER_2);
      _TRENDDIST_2 = 155; //100 v3
      if (a1=="TrendDist_2") return (_TRENDDIST_2);
      _EXITTIMEPROFIT = 11;
      if (a1=="ExitTimeProfit") return (_EXITTIMEPROFIT);
      _MINBANDDIST = 12; //11
      if (a1=="MinBandDist") return (_MINBANDDIST);
      _RESCUEPROFIT = 19;
      if (a1=="RescueProfit") return (_RESCUEPROFIT);
      _FRIDAYENDHOUR = 14;
      if (a1=="FridayEndHour") return (_FRIDAYENDHOUR);
      //v4
      _BreakEntryH4Reverse = 7;
      if (a1=="BreakEntryH4Reverse") return (_BreakEntryH4Reverse);
      _BreakbandH4 = 18;
      if (a1=="BreakbandH4") return (_BreakbandH4);
      _BandDistH4 = 37;
      if (a1=="BandDistH4") return (_BandDistH4);
      _BandPer_H4 = 4;
      if (a1=="BandPer_H4") return (_BandPer_H4);
      _MinBandDistH4 = 15;
      if (a1=="MinBandDistH4") return (_MinBandDistH4);
      _BreakEntrySwingReverse = 0;
      if (a1=="BreakEntrySwingReverse") return (_BreakEntrySwingReverse);
      _SWING_MA_PER = 50;
      if (a1=="SWING_MA_PER") return (_SWING_MA_PER);
      _SWING_MA_DIST = 10;
      if (a1=="SWING_MA_DIST") return (_SWING_MA_DIST);
      _SWING_IMPULSE_MA_PER = 6;
      if (a1=="SWING_IMPULSE_MA_PER") return (_SWING_IMPULSE_MA_PER);
      _SWING_MA_IMPULSE = 0;
      if (a1=="SWING_MA_IMPULSE") return (_SWING_MA_IMPULSE);
      _BandPer_Open_Swing = 10;
      if (a1=="BandPer_Open_Swing") return (_BandPer_Open_Swing);
   }
  Alert("Pairname-",pairname," invalid");
  return(0);
}

int CheckClose(int a0, double a1, double a2, double a3, double a4, int a5, double a6, double a7)
{
   if ((a7 >= 0.0) && (a6 > 0.0))
   {
    if (a4 <= a2)
     {
      if (a4 < a3) return(1);
     }
    else
     return(0);
     
   }
   return (-1);
   
}

bool CheckExitTime(int a0, int a1, double a2, double a3)
{
   if ((a3 >= 0.0) && (a2 > 0.0) && ( a1>(_EXITTIME*60) )) return (true);
   return (false);
}

bool CheckExitTimeFP(int a0, int a1, double atr, double rsi)
{
   if ((rsi >= 0.0) && (atr > 0.0) && ( a1>(_EXITTIMEFORCEPROFIT*60) )) return (true);
   return (false);
}

int GMTOffset()
{
  return (ManualGMT_Offset);
}

int SessionDeinit(int a0, int a1, int a2, int a3, string a4)
{
   return (1);
}
int CheckSignal1(int a0, double a1, double a2, double a3, double a4, double a5, double a6, double a7)
{
   if ((a1 < a4) && (a1 > a5) && (a1 > a6)) return(0);
   if ((a1 > a2) && (a1 < a3) && (a1 < a7)) return(1);
   return (-1);
}

int CheckSignal2(int a0, double a1, double a2, double a3, double a4, double a5, int a6)
{
   if ((a1 < a4) && (a1 > a5) && ((a6==_NOTRENDHOUR1) || (a6==_NOTRENDHOUR2) || (a6==_NOTRENDHOUR3) || (a6==_NOTRENDHOUR4))) return(0);
   if ((a1 > a2) && (a1 < a3) && ((a6==_NOTRENDHOUR1) || (a6==_NOTRENDHOUR2) || (a6==_NOTRENDHOUR3) || (a6==_NOTRENDHOUR4))) return(1);
   return(-1);
}

int CheckSignal3(int a0, double a1, double a2, double a3, double a4, double a5, double a6, double a7, double a8, double a9)
{
   if ((a1 < a3) && (a5 < a7) && (a8 > a9)) return(0);
   if ((a1 > a2) && (a4 > a6) && (a8 > a9)) return(1);
   return(-1);
}

int CheckSignal4(int a0, double a1, double a2, double a3, double a4, double a5)
{
   if ((a1 < a4) && (a1 > a5)) return(0);
   if ((a1 > a2) && (a1 < a3)) return(1);
   return(-1);
}

