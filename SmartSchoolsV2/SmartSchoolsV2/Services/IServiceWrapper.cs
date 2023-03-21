using SmartSchoolsV2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolsV2.Services
{

    public interface IServiceWrapper
    {
        #region Lookup
        Task<string> PostLookupState();
        Task<string> PostLookupCity(int state_id);
        Task<string> PostLookupCountry();
        Task<string> PostLookupCardType();
        Task<string> PostLookupProblemType();
        Task<string> PostLookupUserRace();
        Task<string> PostLookupOccupation();
        Task<string> PostLookupAttendance();
        Task<string> PostLookupReasonForAbsent();
        Task<string> PostLookupPlatformVersion(string platform);
        #endregion

        #region User
        Task<string> PostUserProfile(int profile_id);
        Task<string> PostUserInitLogin(string username);
        Task<string> PostUserLastLogin(string username);
        Task<string> PostUserAuthLogin(string username, string password);
        Task<string> PostUserRoleAccount(string username);
        Task<string> PostUserUpdateDeviceToken(int profile_id, string device_token, int device_platform_id, string update_by);
        Task<string> PostUserChangePassword(int profile_id, string username, string password,string new_password, string update_by);
        Task<string> PostUserCreatePassword(string mobile_number, string username, string password, string new_password);
        Task<string> PostUserChangePin(string username, string new_pin, string update_by);
        Task<string> PostUserForgotPassword(string username, string email);
        Task<string> PostUserUpdateProfile(int profile_id, string full_name, int user_race_id, int card_type_id, string nric,
            string date_of_birth, string mobile_number, string email, string address, string postcode, string city,
            int state_id, string state_name, int country_id, string mother_maiden_name, string occupation, string employer_name,string update_by);
        Task<string> PostUserUpdatePhoto(int profile_id, string file_name, string photo_base64, string update_by);
        Task<string> PostUserRemovePhoto(int profile_id, string update_by);
        Task<string> PostUserUploadImage(string identity_number, int image_type_id, string file_name, string photo_base64, string create_by);
        Task<string> PostUserRegister(string full_name, int nationality_id, int card_type_id,
            string identity_number, string date_of_birth, string mobile_number, string email, string address,
            string postcode, string city, int state_id, int country_id, string mother_maiden_name, string occupation,
            string employer_name, string password, int marketing_flag, int user_role_id);
        Task<string> PostUserVerifyAccount(string mobile_number, string username, string otp);
        Task<string> PostUserResubmitKYC(string username);
        Task<string> PostUserNotify(int profile_id);
        Task<string> PostUserUpdateNotify(int notify_id, int profile_id, string update_by);
        Task<string> PostUserDeleteNotify(int notify_id, int profile_id, string update_by);
        Task<string> PostUserDeleteAccount(string username);
        #endregion

        #region Parent
        Task<string> PostParentProfile(int profile_id);
        Task<string> PostParentSchoolRelationship(int parent_id);
        Task<string> PostParentStudentRelationship(int parent_id);
        Task<string> PostParentCreateStudentRelationship(int parent_id, int student_id, int school_id, int class_id, string create_by);
        Task<string> PostParentRemoveStudentRelationship(int parent_id, int student_id, string update_by);
        Task<string> PostParentSearchStudent(int school_id, string search_name);
        Task<string> PostParentJoinClub(int parent_id, int club_id, string create_by);
        Task<string> PostParentClubRelationship(int parent_id);
        Task<string> PostParentSearchStaff(int school_id, string search_name);
        Task<string> PostParentSearchStaffWallet(int school_id, string search_name);

        #endregion

        #region Staff
        Task<string> PostStaffProfile(int profile_id);
        Task<string> PostStaffClassRelationship(int school_id, int staff_id);
        Task<string> PostStaffClubRelationship(int school_id, int staff_id);
        Task<string> PostStaffCreatePost(int school_id, int post_group_id, int class_id, int club_id, int staff_id, string post_message, string file_name,
            string photo_base64, DateTime start_at, DateTime end_at, string create_by);
        Task<string> PostStaffUpdatePost(int post_id, int school_id, int post_group_id, int class_id, int club_id, int staff_id, string post_message, string file_name,
    string photo_base64, DateTime start_at, DateTime end_at, string update_by);
        Task<string> PostStaffRemovePost(int post_id, int school_id, int post_group_id, int class_id, int club_id, int staff_id, string update_by);
        Task<string> PostStaffRemovePostPhoto(int post_id, int school_id, int post_group_id, int class_id, int club_id, int staff_id, string update_by);
        Task<string> PostStaffLeaveClass(int relationship_id, int staff_id, int class_id, string update_by);
        Task<string> PostStaffJoinClass(int staff_id, int class_id, string class_teacher_flag, string create_by);
        Task<string> PostStaffEnrollStudentClass(int school_id, int student_id, int class_id, string update_by);
        Task<string> PostStaffRemoveStudentClass(int school_id, int student_id, int class_id, string update_by);
        Task<string> PostStaffRemoveClubMember(int relationship_id, int club_id, int profile_id, string update_by);
        Task<string> PostStaffCreateClubAttendance(int school_id, int club_id, string entry_date, string create_by);
        Task<string> PostStaffJoinClub(int staff_id, int club_id, string create_by);
        Task<string> PostStaffCreateClub(string club_name, int school_id, int staff_id, string create_by);
        Task<string> PostStaffHandoverClub(int school_id, int club_id, int current_staff_id, int new_staff_id,int new_profile_id, string update_by);
        Task<string> PostStaffAddClubMember(int club_id, int profile_id, int user_role_id, string create_by);
        Task<string> PostStaffAddClassStudent(int school_id, int class_id, int student_id, string create_by);
        Task<string> PostStaffUpdateStudentAttendance(int report_id, int school_id, int class_id,int student_id, int attendance_id, int reason_id, string update_by);
        Task<string> PostStaffUpdateStaffAttendance(int report_id, int school_id, int staff_id,int attendance_id, int reason_id, string update_by);
        Task<string> PostStaffUpdateClubAttendance(int report_id, int school_id, int club_id, int profile_id, int attendance_id, int reason_id, string update_by);
        Task<string> PostStaffMonthlyAttendance(int school_id, int staff_id, string entry_month);
        Task<string> PostStaffDailyAttendance(int school_id, int staff_id, string entry_date);
        Task<string> PostStaffSearchParent(int school_id, string search_name);
        Task<string> PostStaffSearchStudent(int school_id, string search_name);
        Task<string> PostStaffSearchStudentWallet(int school_id, string search_name);
        Task<string> PostStaffSearchStaff(int school_id, string search_name);
        Task<string> PostStaffSearchMerchant(int school_id, string search_name);
        Task<string> PostStaffShift(int school_id);
        Task<string> PostStaffOutingRequestMonthGroup(int outing_status_id, int school_id);
        Task<string> PostStaffOutingRequestMonth(int outing_status_id, int school_id, DateTime outing_month);
        Task<string> PostStaffOutingRequest(int outing_status_id, int school_id, DateTime outing_month, int outing_type_id);
        Task<string> PostStaffApproveOutingRequest(int outing_id, int student_id, int school_id, int approve_by_id, string approve_comment, string update_by);
        Task<string> PostStaffRejectOutingRequest(int outing_id, int student_id, int school_id, int approve_by_id, string approve_comment, string update_by);
        Task<string> PostStaffRemoveStaff(int school_id, int staff_id, string update_by);
        Task<string> PostStaffUpdateStaffShift(int school_id, int staff_id, int shift_id, string update_by);
        #endregion

        #region Student
        Task<string> PostStudentProfile(int profile_id);
        Task<string> PostStudentJoinClub(int student_id, int club_id, string create_by);
        Task<string> PostStudentClubRelationship(int school_id, int student_id);
        Task<string> PostStudentMonthlyAttendanceSummary(int school_id, int class_id, int student_id, string entry_month);
        Task<string> PostStudentMonthlyAttendance(int school_id, int class_id, int student_id, string entry_month);
        Task<string> PostStudentDailyAttendance(int school_id, int class_id, int student_id, string entry_date);
        Task<string> PostStudentOutingRequestMonthGroup(int student_id, int school_id);
        Task<string> PostStudentOutingRequestMonth(int student_id, int school_id, DateTime outing_month);
        Task<string> PostStudentSaveOutingRequest(int student_id, int school_id, int outing_type_id, DateTime check_out_date, 
            DateTime check_in_date, string outing_reason, int request_by_id, int request_by_user_role_id, string create_by);
        Task<string> PostStudentSubmitOutingRequest(int outing_id, int student_id, int school_id, int outing_type_id, DateTime check_out_date,
            DateTime check_in_date, string outing_reason, int request_by_id, int request_by_user_role_id, string create_by);
        Task<string> PostStudentUpdateOutingRequest(int outing_id, int student_id, int school_id, int outing_type_id, DateTime check_out_date,
    DateTime check_in_date, string outing_reason, int request_by_id, int request_by_user_role_id, string update_by);
        Task<string> PostStudentCancelOutingRequest(int outing_id, int student_id, int school_id, string update_by);

        Task<string> PostStudentParentRelationship(int student_id);
        #endregion

        #region Merchant
        Task<string> PostMerchantProfile(int profile_id);
        Task<string> PostMerchantSchoolRelationship(int merchant_id);
        Task<string> PostMerchantJoinClub(int merchant_id, int club_id, string create_by);
        Task<string> PostMerchantClubRelationship(int merchant_id);
        Task<string> PostMerchantTerminal(int school_id, int merchant_id, string receipt_date);
        Task<string> PostMerchantTerminalReceipt(int school_id, int merchant_id, int terminal_id, string receipt_date);
        Task<string> PostMerchantTerminalReceiptDetail(int rcpt_id, string wallet_number, string reference_number);
        Task<string> PostMerchantProductCategory(int school_id, int merchant_id);
        Task<string> PostMerchantCreateProductCategory(int school_id, int merchant_id, string category_name, string category_description, string create_by);
        Task<string> PostMerchantUpdateProductCategory(int school_id, int merchant_id, int category_id, string category_name, string category_description, string update_by);
        Task<string> PostMerchantUpdateProductNutrition(int info_id, int product_id, string nutrition_name, string per_serving, string update_by);
        Task<string> PostMerchantDeleteProductCategory(int school_id, int merchant_id, int category_id, string update_by);
        Task<string> PostMerchantProductDetail(int school_id, int merchant_id, int category_id);
        Task<string> PostMerchantCreateProductDetail(int merchant_id, int category_id, string product_name, string product_sku,string photo_base64, string file_name,
            decimal unit_price, decimal cost_price, decimal discount_price,string product_description, string product_weight, string product_ingredient, string special_flag, 
            string available_day,string text_color, string background_color, string create_by);
        Task<string> PostMerchantCreateProductNutrition(int product_id, string nutrition_name, string per_serving, string create_by);
        Task<string> PostMerchantUpdateProductDetail(int product_id, int merchant_id, int category_id, string product_name, string product_sku, string photo_base64, string file_name,
            decimal unit_price, decimal cost_price, decimal discount_price, string product_description, string product_weight, string product_ingredient, string special_flag, string available_day, 
            string text_color, string background_color, string update_by);
        Task<string> PostMerchantRemoveProductPhoto(int product_id, int merchant_id, int category_id, string update_by);
        Task<string> PostMerchantDeleteProductDetail(int product_id, int merchant_id, int category_id, string update_by);
        Task<string> PostMerchantProductNutrition(int school_id, int merchant_id, int product_id);
        Task<string> PostMerchantDeleteProductNutrition(int info_id, int product_id, string update_by);
        Task<string> PostMerchantOrderHistory(int school_id, int merchant_id, DateTime pickup_date);
        Task<string> PostMerchantProductOrderHistoryStaff(int order_id, int school_id, DateTime pickup_date);
        Task<string> PostMerchantOrderHistoryGroup(int school_id, int merchant_id);
        Task<string> PostMerchantSettlementGroup(int merchant_id, int school_id);
        Task<string> PostMerchantSettlement(int merchant_id, int school_id, DateTime receipt_date);
        Task<string> PostMerchantStudentOrderHistory(int merchant_id, int school_id, int class_id, DateTime pickup_date);
        Task<string> PostMerchantStudentOrderHistoryDetail(int school_id, int class_id, DateTime pickup_date, int profile_id);
        Task<string> PostMerchantProductOrderHistory(int merchant_id, int school_id, int class_id, DateTime pickup_date);
        Task<string> PostMerchantProductOrderHistoryDetail(int merchant_id, int school_id, int class_id, DateTime pickup_date, int product_id);
        Task<string> PostMerchantSales(int merchant_id);
        Task<string> PostMerchantSalesMethod(int merchant_id, DateTime receipt_date);
        Task<string> PostMerchantUpdateOrderStatus(int school_id, int merchant_id, string order_id, int order_status_id, string update_by, DateTime pickup_date);
        #endregion

        #region School
        Task<string> PostSchoolPost(int[] school_id);
        Task<string> PostSchoolClassPost(int school_id, int class_id);
        Task<string> PostSchoolClubPost(int school_id, int club_id);
        Task<string> PostSchoolClubMember(int school_id, int club_id);
        Task<string> PostSchoolInfo(int school_id);
        Task<string> PostSearchSchool(int state_id, string school_name);
        Task<string> PostSchoolClass(int school_id);
        Task<string> PostSchoolClub(int[] school_id, int create_by_id);
        Task<string> PostSchoolClassStudent(int school_id, int class_id);
        Task<string> PostSchoolStaff(int school_id);
        Task<string> PostSchoolMerchant(int school_id);
        Task<string> PostSchoolClassDailyAttendanceSummary(int school_id, int class_id, string entry_date);
        Task<string> PostSchoolClassDailyAttendancePercentage(int school_id, int class_id, string entry_date);
        Task<string> PostSchoolClassDailyAttendance(int school_id, int class_id, string entry_date);
        Task<string> PostSchoolStaffDailyAttendancePercentage(int school_id, int shift_id, string entry_date);
        Task<string> PostSchoolStaffDailyAttendance(int school_id, int shift_id, string entry_date);
        Task<string> PostSchoolStaffDailyAttendanceSummary(int school_id, int shift_id, string entry_date);
        Task<string> PostSchoolClubDailyAttendancePercentage(int school_id, int club_id, string entry_date);
        Task<string> PostSchoolClubDailyAttendance(int school_id, int club_id, string entry_date);
        Task<string> PostSchoolClubDailyAttendanceSummary(int school_id, int club_id, string entry_date);
        Task<string> PostSchoolClubMemberMonthlyAttendance(int school_id, int club_id, int profile_id, string entry_month);
        Task<string> PostSchoolClubMemberDailyAttendance(int school_id, int club_id, int profile_id, string entry_date);
        #endregion

        #region Wallet
        Task<string> PostAccountTopup(int wallet_id, string wallet_number, decimal topup_amount, string topup_date, string create_by);
        Task<string> PostWalletTopup(int wallet_id, string wallet_number, decimal topup_amount, string topup_date, string create_by);
        Task<string> PostWalletTransfer(int wallet_id, string wallet_number, int recipient_id, string recipient_wallet_number, decimal transfer_amount, string transfer_date, string create_by);
        Task<string> PostWalletTransactionHistory(string wallet_number);
        Task<string> PostWalletTransactionReference(int transaction_id, string reference_number);
        Task<string> PostWalletTransactionDetail(string wallet_number, string reference_number);
        Task<string> PostWalletTransactionMaster(string wallet_number, string reference_number);
        Task<string> PostAccountCardInfo(string username);
        Task<string> PostAccountTopupStatus(string username, string reference_id, string update_by);
        Task<string> PostAccountRegisterCheck(string username, int user_role_id);

        #endregion

        #region Card

        Task<string> PostCardDailyLimit(int card_id);
        Task<string> PostCardUpdateDailyLimit(int card_id, int school_id, decimal daily_limit, string update_by);
        Task<string> PostCardUpdateStatusBlacklist(int card_id, int school_id, string update_by);
        Task<string> PostCardReplacement(int user_role_id, int school_id, int old_card_id, int old_card_status_id, string new_card_number, string create_by);
        Task<string> PostCardSearchAssignment(int user_role_id, int school_id, string search_name);
        #endregion

        #region General
        Task<string> PostGeneralLeaveClub(int relationship_id, int profile_id, int club_id, string update_by);
        #endregion

        #region Cart
        Task<string> PostPurchaseProductCategory(int school_id, int merchant_id, string special_flag);
        Task<string> PostPurchaseProductDetail(int school_id, int merchant_id, int category_id, string special_flag, string available_day);
        Task<string> PostPurchaseCartCount(int profile_id, int wallet_id);
        Task<string> PostPurchaseCartTotal(int profile_id, int wallet_id);
        Task<string> PostPurchaseOrderHistory(int profile_id, int wallet_id);
        Task<string> PostPurchaseDetailOrderHistory(int order_id, int wallet_id);
        Task<string> PostPurchasePlaceOrder(int profile_id, int wallet_id, int order_status_id, int user_role_id, decimal sub_total_amount, decimal tax_amount, decimal total_amount, int payment_method_id, string create_by);
        Task<string> PostPurchaseCart(int profile_id, int wallet_id, int order_status_id);
        Task<string> PostPurchaseCartGroupDate(int profile_id, int wallet_id, int order_status_id);
        Task<string> PostPurchaseCartPickupDate(int profile_id, int wallet_id, int order_status_id, DateTime pickup_date);
        Task<string> PostPurchaseInsertCart(int profile_id, int wallet_id, int merchant_id, int school_id, int user_role_id, int recipient_id,
            int recipient_role_id, DateTime pickup_date, TimeSpan pickup_time, int service_method_id, string delivery_location, int product_id, int product_qty, string create_by);
        Task<string> PostPurchaseUpdateCart(int cart_id, int profile_id, int wallet_id, int merchant_id, int school_id, int recipient_id,
            DateTime pickup_date, int product_id, int product_qty, string update_by);
        Task<string> PostPurchaseDeleteCart(int cart_id, int profile_id, int recipient_id, int product_id, string update_by);
        Task<string> PostPurchaseOrderHistoryDetailGroupDate(string wallet_number, string reference_number);
        Task<string> PostPurchaseOrderHistoryDetailDate(string wallet_number, string reference_number, DateTime pickup_date);
        Task<string> PostPurchaseOrderHistoryTotal(string wallet_number, string reference_number);
        Task<string> PostPurchaseRemoveOrderMaster(int order_id, int profile_id, int wallet_id, string update_by);
        Task<string> PostPurchaseContinueOrderMaster(int order_id, int profile_id, int wallet_id, string update_by);
        #endregion

        #region Contact
        Task<string> PostContactClassTeacher(int parent_id);
        Task<string> PostContactStudentParentStaffMerchant(int school_id, int[] class_id);
        Task<string> PostContactSchoolStaffMerchant(int school_id, int profile_id);
        Task<string> PostContactSchoolStaff(int[] school_id);
        Task<string> PostContactStudentClass(int parent_id);
        #endregion

        #region Report
        Task<string> PostReportCustomerFeedback(int support_category_id, int problem_type_id, int priority_type_id, string ticket_subject, string ticket_desc, int ticket_status_id, string create_by);
        
        Task<string> PostReportUploadAttachment(int ticket_id, string file_name, string photo_base64, string create_by);

        Task<string> PostReportClassAttendance(int school_id, int class_id, DateTime entry_month, string create_by);

        Task<string> PostReportStaffAttendance(int school_id, int shift_id, DateTime entry_month, string create_by);

        Task<string> PostReportClassOrder(int merchant_id, int school_id, DateTime pickup_date, string create_by);

        Task<string> PostReportStaffOrder(int merchant_id, int school_id, DateTime pickup_date, string create_by);

        Task<string> PostReportStudentOrder(int merchant_id, int school_id, int class_id, DateTime pickup_date, string create_by);

        Task<string> PostReportProductOrder(int merchant_id, int school_id, DateTime pickup_date, string create_by);
        #endregion

        #region Chat
        Task<string> PostChatUserStatus(string channel_id, int profile_id, int status_id, string update_by);
        Task<string> PostChatJoinChannel(string channel_id, int profile_id, string create_by);
        Task<string> PostChatLeaveChannel(string channel_id, int profile_id, string update_by);
        Task<string> PostChatHistory(int profile_id);
        Task<string> PostChatChannelUser(string channel_id);
        Task<string> PostChatSend(string channel_id, int channel_type_id, string channel_name, int sender_id, int receiver_id, int message_type_id, string message, string photo_base64, string file_name, string create_by);
        #endregion

        #region Account
        Task<string> PostAccountRegister(int user_role_id, string full_name, int card_type_id, int nationality_id, string date_of_birth,
        string identity_number, string mobile_number, string email, string password, int marketing_flag);

        //Task<string> PostAccountVerify(int account_id, string otp);
        Task<string> PostAccountVerify(string mobile_number, string username, string otp);
        Task<string> PostAccountVerifyKYC(string username);
        Task<string> PostAccountResubmitKYC(string username);
        Task<string> PostAccountStatus(int profile_id, string wallet_number);
        Task<string> PostAccountUpdateVirtualBalance(int parent_profile_id, int student_profile_id, string create_by);
        Task<string> PostAccountAddVirtualBalance(int parent_id, int student_id, int school_id, int class_id, string create_by);
        Task<string> PostAccountUpdateProfile(int profile_id, string full_name, string nric,
        string address, string postcode, string city, int state_id, int country_id,
        string mother_maiden_name, string occupation, string employer_name, string update_by);
        Task<string> PostAccountInfo(int profile_id, int user_role_id);
        #endregion
    }
}
