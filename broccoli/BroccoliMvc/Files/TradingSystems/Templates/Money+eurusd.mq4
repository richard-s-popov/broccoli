
#property copyright "www.broccoli-trade.ru"
#property link      "http://www.broccoli-trade.ru"

string g_comment_76 = "Money+eurusd";
extern double Lot = 0.1;
int gi_92 = 0;
extern int StopLoss = 4;
int gi_100 = 1;
int gi_unused_104 = 0;
int gi_108 = 3;
int gi_112 = 0;
double gd_116 = 10.0;
int g_period_124 = 15;
double gd_128 = 2.0;
// string gs_136 = "2011.06.28";
int gi_144 = 31;
extern int MAGIC = 80050;
double gd_152;
int gi_160 = 2;
int g_slippage_164;
double g_maxlot_168;
double g_minlot_176;
double g_lotstep_184;
int gi_192;
bool gi_196 = TRUE;
datetime g_time_200;
double gd_204;
bool gi_unused_212 = TRUE;
bool gi_216;
bool gi_220;
int gi_unused_224;
double gda_228[100];
double gda_232[100];
int account_number=##account_number;
int init() {
   if (IsTesting() && !IsVisualMode()) gi_unused_212 = FALSE;
   if (!gi_196) return (0);
   g_maxlot_168 = MarketInfo(Symbol(), MODE_MAXLOT);
   g_minlot_176 = MarketInfo(Symbol(), MODE_MINLOT);
   g_lotstep_184 = MarketInfo(Symbol(), MODE_LOTSTEP);
   if (g_lotstep_184 == 0.1) gi_192 = 1;
   if (g_lotstep_184 == 0.01) gi_192 = 2;
   g_slippage_164 = gi_160;
   gd_152 = Point;
   if (Digits == 5 || Digits == 3) {
      gd_152 = 10.0 * gd_152;
      g_slippage_164 = 10 * g_slippage_164;
   }
   return (0);
}

int deinit() {
   Comment("");
   return (0);
}

int start() {
if (AccountNumber()!=account_number)
   {
   Alert("Неверный счет... советник не работает");
   return(0);
   }
   if (!gi_196) return (0);
   if (!gi_196) return (0);
   gd_204 = MarketInfo(Symbol(), MODE_STOPLEVEL) * Point / gd_152;
   if (g_time_200 < Time[0] || gi_100 > 1) {
      Indic();
      g_time_200 = Time[0];
      gi_216 = FALSE;
      gi_220 = FALSE;
      if (gda_232[2] == 0.0 && gda_232[1] != 0.0) gi_220 = TRUE;
      if (gda_228[2] == 0.0 && gda_228[1] != 0.0) gi_216 = TRUE;
      if (gi_100 > 1) {
         if (gda_232[1] == 0.0 && gda_232[0] != 0.0) {
            if (!IsOptimization()) Alert("Close Buy Signal!");
            CloseOrders(OP_BUY);
         }
         if (gda_228[1] == 0.0 && gda_228[0] != 0.0) {
            if (!IsOptimization()) Alert("Close Sell Signal!");
            CloseOrders(OP_SELL);
         }
      }
      if (gi_216) {
         if (gi_100 == 1) {
            if (iCountMarket_Orders(OP_SELL) > 0) {
               if (!IsOptimization()) Alert("Close Sell Signal!");
               CloseOrders(OP_SELL);
            }
         }
         if (iCountMarket_Orders() == 0) {
            if (!IsOptimization()) Alert("Open Buy Signal!");
            vOpenMarketOrder(OP_BUY);
         }
      }
      if (gi_220) {
         if (gi_100 == 1) {
            if (iCountMarket_Orders(OP_BUY) > 0) {
               if (!IsOptimization()) Alert("Close Buy Signal!");
               CloseOrders(OP_BUY);
            }
         }
         if (iCountMarket_Orders() == 0) {
            if (!IsOptimization()) Alert("Open Sell Signal!");
            vOpenMarketOrder(OP_SELL);
         }
      }
   }
   gd_204 = MarketInfo(Symbol(), MODE_STOPLEVEL) * Point / gd_152;
   return (0);
}

void vOpenMarketOrder(int a_cmd_0) {
   color l_color_12;
   double l_price_20;
   string ls_28;
   int l_ticket_36;
   double l_price_40;
   double l_price_48;
   double ld_4 = dAutoLot();
   if (AccountFreeMarginCheck(Symbol(), a_cmd_0, ld_4) <= 0.0) {
      if (!IsOptimization()) Alert("We have no money for " + ld_4 + " lots!");
      gi_196 = FALSE;
      return;
   }
   if (a_cmd_0 == OP_BUY) l_color_12 = Blue;
   if (a_cmd_0 == OP_SELL) l_color_12 = Red;
   for (int l_count_16 = 0; l_count_16 <= gi_108; l_count_16++) {
      if (!IsTesting()) {
         while (!IsTradeAllowed()) Sleep(5000);
         RefreshRates();
      }
      if (a_cmd_0 == OP_BUY) {
         l_price_20 = ND(Ask);
         ls_28 = "Buy";
      }
      if (a_cmd_0 == OP_SELL) {
         l_price_20 = ND(Bid);
         ls_28 = "Sell";
      }
      l_ticket_36 = OrderSend(Symbol(), a_cmd_0, ld_4, l_price_20, g_slippage_164, 0, 0, g_comment_76, MAGIC, 0, l_color_12);
      if (l_ticket_36 > 0) {
         if (!(OrderSelect(l_ticket_36, SELECT_BY_TICKET, MODE_TRADES))) break;
         if (!IsOptimization()) Print("Opened order ", ls_28, " #", l_ticket_36);
         if (StopLoss > 0) {
            if (a_cmd_0 == OP_BUY) l_price_40 = dGet_SL(0, OrderOpenPrice());
            if (a_cmd_0 == OP_SELL) l_price_40 = dGet_SL(1, OrderOpenPrice());
            if (!OrderModify(l_ticket_36, OrderOpenPrice(), l_price_40, 0, 0, l_color_12))
               if (!IsOptimization()) Print("Start StopLoss modify Error", GetLastError());
         }
         if (gi_92 <= 0) break;
         if (a_cmd_0 == OP_BUY) l_price_48 = dGet_TP(0, OrderOpenPrice());
         if (a_cmd_0 == OP_SELL) l_price_48 = dGet_TP(1, OrderOpenPrice());
         if (!(!OrderModify(l_ticket_36, OrderOpenPrice(), l_price_40, l_price_48, 0, l_color_12))) break;
         if (!(!IsOptimization())) break;
         Print("Start TakeProfit modify Error", GetLastError());
         return;
      }
      if (iCheckError(GetLastError(), a_cmd_0) != 1) break;
   }
}

double dAutoLot() {
   double ld_ret_0;
   double l_marginrequired_8;
   double ld_16;
   if (gi_112 == 0) ld_ret_0 = Lot;
   if (gi_112 == 1) ld_ret_0 = NormalizeDouble(AccountEquity() / 100000.0 * gd_116, gi_192);
   if (gi_112 == 2) ld_ret_0 = NormalizeDouble(AccountFreeMargin() / 100000.0 * gd_116, gi_192);
   if (gi_112 == 3) ld_ret_0 = NormalizeDouble(AccountBalance() / 100000.0 * gd_116, gi_192);
   if (gi_112 == 4) {
      l_marginrequired_8 = MarketInfo(Symbol(), MODE_MARGINREQUIRED);
      ld_16 = AccountEquity() * gd_116 / 100.0;
      ld_ret_0 = NormalizeDouble(ld_16 / l_marginrequired_8, gi_192);
   }
   if (ld_ret_0 > g_maxlot_168) ld_ret_0 = g_maxlot_168;
   if (ld_ret_0 < g_minlot_176) ld_ret_0 = g_minlot_176;
   return (ld_ret_0);
}

double dGet_TP(int ai_0, double ad_4) {
   double ld_ret_12;
   if (ai_0 == 0) {
      if (gi_92 <= 0) ld_ret_12 = 0;
      else ld_ret_12 = ND(ad_4 + gi_92 * gd_152);
      if (!IsTesting()) RefreshRates();
      if (ld_ret_12 < ND(Bid + gd_204 * gd_152) && ld_ret_12 > 0.0) ld_ret_12 = ND(Bid + gd_204 * gd_152);
   }
   if (ai_0 == 1) {
      if (gi_92 <= 0) ld_ret_12 = 0;
      else ld_ret_12 = ND(ad_4 - gi_92 * gd_152);
      if (!IsTesting()) RefreshRates();
      if (ld_ret_12 > ND(Ask - gd_204 * gd_152)) ld_ret_12 = ND(Ask - gd_204 * gd_152);
   }
   return (ld_ret_12);
}

double dGet_SL(int ai_0, double ad_4) {
   double ld_ret_12;
   if (ai_0 == 0) {
      if (StopLoss > 0) ld_ret_12 = ND(ad_4 - StopLoss * gd_152);
      else ld_ret_12 = 0;
      if (!IsTesting()) RefreshRates();
      if (ld_ret_12 > ND(Bid - gd_204 * gd_152)) ld_ret_12 = ND(Bid - gd_204 * gd_152);
   }
   if (ai_0 == 1) {
      if (StopLoss > 0) ld_ret_12 = ND(ad_4 + StopLoss * gd_152);
      else ld_ret_12 = 0;
      if (!IsTesting()) RefreshRates();
      if (ld_ret_12 < ND(Ask + gd_204 * gd_152) && ld_ret_12 > 0.0) ld_ret_12 = ND(Ask + gd_204 * gd_152);
   }
   return (ld_ret_12);
}

int iCheckError(int ai_0, int ai_4) {
   switch (ai_0) {
   case 4:
      Alert(Symbol(), ": Trade server is busy. Try repeat...");
      Sleep(3000);
      return (1);
   case 6:
      Alert(Symbol(), ": No connection with trade server. Try repeat...");
      Sleep(5000);
      return (1);
   case 128:
      Alert(Symbol(), ": Trade timeout. Try repeat...");
      Sleep(66000);
      if (ai_4 <= 1)
         if (iCountMarket_Orders(ai_4) > 0) return (0);
      return (1);
   case 130:
      Alert(Symbol(), ": Invalid stops. Try repeat...");
      gd_204 += 0.5;
      return (1);
   case 142:
      Alert(Symbol(), ": Trade timeout. Try repeat...");
      Sleep(66000);
      if (ai_4 <= 1)
         if (iCountMarket_Orders(ai_4) > 0) return (0);
      return (1);
   case 143:
      Alert(Symbol(), ": Trade timeout. Try repeat...");
      Sleep(66000);
      if (ai_4 <= 1)
         if (iCountMarket_Orders(ai_4) > 0) return (0);
      return (1);
   case 129:
      Alert(Symbol(), ": Invalid price. Try repeat...");
      Sleep(3000);
      return (1);
   case 135:
      Alert(Symbol(), ": Price changed. Try repeat...");
      RefreshRates();
      return (1);
   case 136:
      Alert(Symbol(), ": Off quotes. Wait new tick...");
      while (!RefreshRates()) Sleep(1);
      return (1);
   case 137:
      Alert(Symbol(), ": Broker is busy. Try repeat...");
      Sleep(3000);
      return (1);
   case 138:
      Alert(Symbol(), ": Requote. Try repeat...");
      Sleep(5000);
      return (1);
   case 146:
      Alert(Symbol(), ": Trade context is busy. Try repeat...");
      Sleep(500);
      return (1);
   case 2:
      Alert("Common error.");
      return (0);
   case 5:
      Alert("Old version of the client terminal.");
      gi_196 = FALSE;
      return (0);
   case 64:
      Alert("Account disabled.");
      gi_196 = FALSE;
      return (0);
   case 133:
      Alert("Trade is disabled.");
      return (0);
   case 134:
      Alert(Symbol(), ": Not enough money.");
      return (0);
   }
   Alert(Symbol(), ": Is other error ", ai_0);
   return (0);
}

double ND(double ad_0) {
   return (NormalizeDouble(ad_0, Digits));
}

int iCountMarket_Orders(int a_cmd_0 = -1) {
   int l_count_4 = 0;
   int l_ord_total_8 = OrdersTotal();
   for (int l_pos_12 = 0; l_pos_12 < l_ord_total_8; l_pos_12++) {
      if (OrderSelect(l_pos_12, SELECT_BY_POS, MODE_TRADES)) {
         if (OrderSymbol() == Symbol() && OrderMagicNumber() == MAGIC && OrderType() <= OP_SELL)
            if (OrderType() == a_cmd_0 || a_cmd_0 == -1) l_count_4++;
      }
   }
   return (l_count_4);
}

void CloseOrders(int a_cmd_0 = -1) {
   color l_color_8;
   double l_price_12;
   string ls_20;
   bool l_ord_close_28;
   for (int l_pos_4 = OrdersTotal() - 1; l_pos_4 >= 0; l_pos_4--) {
      OrderSelect(l_pos_4, SELECT_BY_POS, MODE_TRADES);
    // while (iCheckError(GetLastError(), OrderType()) == 1) 
     {
            if (!IsTesting()) {
            while (!IsTradeAllowed()) Sleep(5000);
            RefreshRates();
         }
         if (OrderType() == OP_BUY) {
            l_color_8 = Blue;
            l_price_12 = ND(Bid);
            ls_20 = "Buy";
            if (l_price_12 >= OrderTakeProfit() && OrderTakeProfit() != 0.0 || OrderTicket() == 0) return;
         }
         if (OrderType() == OP_SELL) {
            l_color_8 = Red;
            l_price_12 = ND(Ask);
            ls_20 = "Sell";
            if (l_price_12 <= OrderTakeProfit() || OrderTicket() == 0) return;
         }
         l_ord_close_28 = OrderClose(OrderTicket(), OrderLots(), l_price_12, g_slippage_164, l_color_8);
         if (l_ord_close_28) {
            if (!(!IsOptimization())) break;
            Print("Closed order ",ls_20," #",OrderTicket());
            break;
         }
      }
   }
}

void Indic() {
   bool li_0;
   bool li_4;
   int lia_8[100];
   double lda_12[100];
   double lda_16[100];
   double ld_20;
   double l_iatr_28;
   for (int li_36 = 100; li_36 >= 0; li_36--) {
      gda_228[li_36] = 0;
      gda_232[li_36] = 0;
      l_iatr_28 = iATR(NULL, 0, g_period_124, li_36);
      ld_20 = (High[li_36] + Low[li_36]) / 2.0;
      lda_12[li_36] = ld_20 + gd_128 * l_iatr_28;
      lda_16[li_36] = ld_20 - gd_128 * l_iatr_28;
      lia_8[li_36] = 1;
      if (Close[li_36] > lda_12[li_36 + 1]) {
         lia_8[li_36] = 1;
         if (lia_8[li_36 + 1] == -1) gi_unused_224 = 1;
      } else {
         if (Close[li_36] < lda_16[li_36 + 1]) {
            lia_8[li_36] = -1;
            if (lia_8[li_36 + 1] == 1) gi_unused_224 = 1;
         } else {
            if (lia_8[li_36 + 1] == 1) {
               lia_8[li_36] = 1;
               gi_unused_224 = 0;
            } else {
               if (lia_8[li_36 + 1] == -1) {
                  lia_8[li_36] = -1;
                  gi_unused_224 = 0;
               }
            }
         }
      }
      if (lia_8[li_36] < 0 && lia_8[li_36 + 1] > 0) li_0 = TRUE;
      else li_0 = FALSE;
      if (lia_8[li_36] > 0 && lia_8[li_36 + 1] < 0) li_4 = TRUE;
      else li_4 = FALSE;
      if (lia_8[li_36] > 0 && lda_16[li_36] < lda_16[li_36 + 1]) lda_16[li_36] = lda_16[li_36 + 1];
      if (lia_8[li_36] < 0 && lda_12[li_36] > lda_12[li_36 + 1]) lda_12[li_36] = lda_12[li_36 + 1];
      if (li_0 == TRUE) lda_12[li_36] = ld_20 + gd_128 * l_iatr_28;
      if (li_4 == TRUE) lda_16[li_36] = ld_20 - gd_128 * l_iatr_28;
      if (lia_8[li_36] == 1) gda_228[li_36] = lda_16[li_36];
      else
         if (lia_8[li_36] == -1) gda_232[li_36] = lda_12[li_36];
   }
}