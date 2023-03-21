using System;
using System.Collections.Generic;

namespace SmartSchoolsV2.Models
{

    public class ResponseProperty 
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class LoginProperty
    {
        public string access_token { get; set; }
        public string access_type { get; set; }
        public string last_login { get; set; }
        public int expires_in { get; set; }
    }

    public class RoleAccountProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public List<RoleAccount> Data { get; set; }
    }

    public class RoleAccount
    {
        public int user_role_id { get; set; }
        public string user_role { get; set; }
    }

    public class UserProfileProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<UserProfile> Data { get; set; }
    }

    public class UserProfile
    {
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public string email { get; set; }
        public string mobile_number { get; set; }
        public int user_race_id { get; set; }
        public string user_race { get; set; }
        public int card_type_id { get; set; }
        public string card_type { get; set; }
        public string nric { get; set; }
        public string date_of_birth { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public string city { get; set; }
        public int state_id { get; set; }
        public string state_name { get; set; }
        public int country_id { get; set; }
        public string country_name { get; set; }
        public string mother_maiden_name { get; set; }
        public string occupation { get; set; }
        public string employer_name { get; set; }
    }

    public class ParentProfileProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ParentProfile> Data { get; set; }
    }

    public class AccStatusProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<AccStatus> Data { get; set; }
    }
    public class AccStatus
    {
        public string mpay_uid { get; set; }
        public string account_status_id { get; set; }
        public string account_status { get; set; }
        public string kyc_status_id { get; set; }
        public string kyc_status { get; set; }
    }

    public class ParentProfile
    {
        public int parent_id { get; set; }
        public string full_name { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public decimal account_balance { get; set; }
        public string photo_url { get; set; }
    }

    public class StudentProfileProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentProfile> Data { get; set; }
    }

    public class ReportProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class StudentProfile
    {
        public int student_id { get; set; }
        public int card_id { get; set; }
        public int card_status_id { get; set; }
        public string card_status { get; set; }
        public string card_number { get; set; }
        public string full_name { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public decimal account_balance { get; set; }
        public string photo_url { get; set; }
        public string school_name { get; set; }
        public string class_name { get; set; }
    }

    public class CardProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Card> Data { get; set; }
    }

    public class Card
    {
        public int card_id { get; set; }
        public string card_number { get; set; }
        public int card_status_id { get; set; }
        public string card_status { get; set; }
        public decimal daily_limit { get; set; }
    }

    public class MerchantProfileProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MerchantProfile> Data { get; set; }
    }

    public class MerchantProfile
    {
        public int merchant_id { get; set; }
        public string full_name { get; set; }
        public int merchant_type_id { get; set; }
        public string company_name { get; set; }
        public string registration_number { get; set; }
        public string photo_url { get; set; }
    }

    public class SchoolMerchantProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolMerchant> Data { get; set; }
    }

    public class SchoolMerchant
    {
        public int merchant_id { get; set; }
        public string full_name { get; set; }
        public int merchant_type_id { get; set; }
        public string merchant_type { get; set; }
        public string company_name { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
        public string photo_url { get; set; }
        public string search_name { get; set; }
        public string search_name2 { get; set; }
        public string search_name3 { get; set; }
    }

    public class StaffProfileProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffProfile> Data { get; set; }
    }

    public class StaffProfile
    {
        public int staff_id { get; set; }
        public string staff_number { get; set; }
        public string full_name { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int shift_id { get; set; }
        public string shift_code { get; set; }
        public int staff_type_id { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public decimal account_balance { get; set; }
        public string photo_url { get; set; }
        public int card_id { get; set; }
        public string card_number { get; set; }
        public int card_status_id { get; set; }
        public string card_status { get; set; }
    }

    public class ParentSchoolRelationshipProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ParentSchoolRelationship> Data { get; set; }
    }

    public class ParentSchoolRelationship
    {
        public int school_id { get; set; }
    }

    public class ParentStudentRelationshipProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ParentStudentRelationship> Data { get; set; }
    }

    public class ParentStudentRelationship
    {
        public int student_id { get; set; }
        public int profile_id { get; set; }
        public string student_number { get; set; }
        public string photo_url { get; set; }
        public string photo_url_student { get; set; }
        public string full_name { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int card_id { get; set; }
        public string card_number { get; set; }
        public string account_balance { get; set; }
        public int card_status_id { get; set; }
        public string card_status { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
        public bool dot_visible { get; set; }
        public bool is_selected { get; set; }
    }

    public class StudentParentRelationshipProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentParentRelationship> Data { get; set; }
    }

    public class StudentParentRelationship
    {
        public int parent_id { get; set; }
        public int profile_id { get; set; }
        public string photo_url { get; set; }
        public string full_name { get; set; }
        public string mobile_number { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }

    }

    public class SchoolClassStudentProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolClassStudent> Data { get; set; }
    }

    public class SchoolClassStudent
    {
        public int student_id { get; set; }
        public int profile_id { get; set; }
        public string student_number { get; set; }
        public string photo_url { get; set; }
        public string photo_url_student { get; set; }
        public string full_name { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
        public bool dot_visible { get; set; }
    }

    public class CardAssignmentProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<CardAssignment> Data { get; set; }
    }

    public class CardAssignment
    {
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int card_id { get; set; }
        public string card_number { get; set; }
        public int card_status_id { get; set; }
        public string card_status { get; set; }
        public string search_name { get; set; }
        public string search_name2 { get; set; }
        public string search_name3 { get; set; }
        public string school_name { get; set; }
        public string class_name { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
    }

    public class SchoolStaffProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolStaff> Data { get; set; }
    }

    public class SchoolStaff
    {
        public int staff_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string staff_number { get; set; }
        public string photo_url_staff { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public int staff_type_id { get; set; }
        public string staff_type { get; set; }
        public string staff_type_bm { get; set; }
        public int shift_id { get; set; }
        public string shift_code { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
        public bool dot_visible { get; set; }
        public string search_name { get; set; }
        public string search_name2 { get; set; }
    }


    public class SchoolPostProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolPost> Data { get; set; }
    }

    public class SchoolPost
    {
        public int post_id { get; set; }
        public string school_name { get; set; }
        public string school_type { get; set; }
        public string post_message { get; set; }
        public string post_message64 { get; set; }
        public string post_photo_url { get; set; }
        public string create_by { get; set; }
        public string create_by_photo_url { get; set; }
        public string create_at { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
        public bool post_photo_visible { get; set; }
        public bool edit_visible { get; set; }
        public string date_from { get; set; }
        public string date_to { get; set; }
    }

    public class StaffClassRelationshipProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffClassRelationship> Data { get; set; }
    }

    public class StaffClassRelationship
    {
        public int relationship_id { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public string school_type { get; set; }
        public string session_code { get; set; }
        public string class_teacher_flag { get; set; }
        public string total_student { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
    }

    public class ClubRelationshipProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClubRelationship> Data { get; set; }
    }

    public class ClubRelationship
    {
        public int relationship_id { get; set; }
        public int club_id { get; set; }
        public string club_name { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public string school_type { get; set; }
        public string total_member { get; set; }
        public int create_by_staff_id { get; set; }
    }

    public class SchoolInfoProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolInfo> Data { get; set; }
    }

    public class SchoolInfo
    {
        public int school_id { get; set; }
        public string school_name { get; set; }
        public string school_code { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public string email { get; set; }
        public string school_website { get; set; }
        public string school_result_url { get; set; }
        public string city { get; set; }
        public string state_name { get; set; }
        public string total_staff { get; set; }
    }

    public class TransactionHistoryProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<TransactionHistory> Data { get; set; }
    }

    public class TransactionHistory
    {
        public int transaction_id { get; set; }
        public string reference_number { get; set; }
        public int transaction_type_id { get; set; }
        public string transaction_type { get; set; }
        public string transaction_type_bm { get; set; }
        public string transaction_method { get; set; }
        public string wallet_number { get; set; }
        public string full_name { get; set; }
        public string wallet_number_reference { get; set; }
        public string full_name_reference { get; set; }
        public string amount { get; set; }
        public string amount_color { get; set; }
        public int status_id { get; set; }
        public string status_code { get; set; }
        public string status_code_bm { get; set; }
        public string create_at { get; set; }
        public string create_month { get; set; }
    }

    public class TerminalReceiptProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<TerminalReceipt> Data { get; set; }
    }

    public class TerminalReceipt
    {
        public int rcpt_id { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public string reference_number { get; set; }
        public string full_name { get; set; }
        public string receipt_time { get; set; }
        public string receipt_date { get; set; }
        public string total_amount { get; set; }
        public string payment_method { get; set; }
        public string payment_method_bm { get; set; }
        public string company_name { get; set; }
    }

    public class TransactionDetailProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<TransactionDetail> Data { get; set; }
    }
    public class TransactionDetail
    {
        public int rcpt_detail_id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_qty { get; set; }
        public string unit_price { get; set; }
        public string sub_total_amount { get; set; }
        public string total_amount { get; set; }
    }

    public class CountryProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Country> Data { get; set; }
    }

    public class Country
    {
        public int country_id { get; set; }
        public string country_name { get; set; }
        public string locale_code { get; set; }
        public string country_code { get; set; }
        public int search_id { get; set; }
        public string search_name { get; set; }
    }

    public class StateProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<State> Data { get; set; }
    }

    public class State
    {
        public int state_id { get; set; }
        public string state_name { get; set; }
        public int search_id { get; set; }
        public string search_name { get; set; }
    }

    public class CityProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<City> Data { get; set; }
    }

    public class City
    {
        public int city_id { get; set; }
        public string city_name { get; set; }
        public int search_id { get; set; }
        public string search_name { get; set; }
    }

    public class OccupationProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Occupation> Data { get; set; }
    }

    public class Occupation
    {
        public int occupation_id { get; set; }
        public string occupation { get; set; }
        public int search_id { get; set; }
        public string search_name { get; set; }
    }

    public class StaffShiftProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffShift> Data { get; set; }
    }

    public class StaffShift
    {
        public int shift_id { get; set; }
        public string shift_code { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
        public int search_id { get; set; }
        public string search_name { get; set; }
    }

    public class UserRaceProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<UserRace> Data { get; set; }
    }

    public class CardType
    {
        public int card_type_id { get; set; }
        public string card_type { get; set; }
        public int search_id { get; set; }
        public string search_name { get; set; }
    }

    public class CardTypeProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<CardType> Data { get; set; }
    }

    public class UserRace
    {
        public int user_race_id { get; set; }
        public string user_race { get; set; }
        public int search_id { get; set; }
        public string search_name { get; set; }
    }

    public class ReasonForAbsentProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ReasonForAbsent> Data { get; set; }
    }

    public class ReasonForAbsent
    {
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
        public int search_id { get; set; }
        public string search_name { get; set; }
    }

    public class SchoolProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<School> Data { get; set; }
    }

    public class School
    {
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public string school_code { get; set; }
        public string city { get; set; }
        public string country_name { get; set; }
        public string status_code { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
        public int search_id { get; set; }
        public string search_name { get; set; }
        public string search_name2 { get; set; }
    }

    public class SchoolClassProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolClass> Data { get; set; }
    }

    public class SchoolClass
    {
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int search_id { get; set; }
        public string search_name { get; set; }
        public string search_name2 { get; set; }
        public string session_code { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
    }

    public class SchoolClubProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolClub> Data { get; set; }
    }

    public class SchoolClub
    {
        public int club_id { get; set; }
        public string club_name { get; set; }
        public string school_name { get; set; }
        public int search_id { get; set; }
        public string search_name { get; set; }
        public string search_name2 { get; set; }
        public string full_name { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
    }

    public class StudentProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Student> Data { get; set; }
    }

    public class Student
    {
        public int student_id { get; set; }
        public string student_number { get; set; }
        public string photo_url { get; set; }
        public string full_name { get; set; }
        public string nric { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int card_id { get; set; }
        public string card_number { get; set; }
        public string card_status { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
        public string search_name { get; set; }
        public string search_name2 { get; set; }
    }

    public class CrudProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
    public class CartProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public int Total { get; set; }
    }
    //public class CartTotalProperty
    //{
    //    public bool Success { get; set; }
    //    public string Code { get; set; }
    //    public string Message { get; set; }
    //    public decimal Total { get; set; }
    //}

    public class CartTotalProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<CartTotal> Data { get; set; }
    }

    public class CartTotal
    {
        public decimal sub_total_amount { get; set; }
        public int tax_rate { get; set; }
        public decimal tax_amount { get; set; }
        public decimal total_amount { get; set; }
    }

    public class CardInfoProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<CardInfo> Data { get; set; }
    }

    public class CardInfo
    {
        public string balance { get; set; }
    }

    public class RegisterAccountProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<RegisterAccount> Data { get; set; }
    }

    public class RegisterAccount
    {
        public int account_id { get; set; }
    }
    public class ClubMemberProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClubMember> Data { get; set; }
    }

    public class ClubMember
    {
        public int relationship_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int user_role_id { get; set; }
        public string user_role { get; set; }
        public string status_code { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
    }

    public class MerchantSchoolRelationshipProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MerchantSchoolRelationship> Data { get; set; }
    }

    public class MerchantSchoolRelationship
    {
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int school_type_id { get; set; }
        public string school_type { get; set; }
        public string state_name { get; set; }
        public string city { get; set; }
        public string country_name { get; set; }
    }

    public class MerchantTerminalProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MerchantTerminal> Data { get; set; }
    }

    public class MerchantTerminal
    {
        public int terminal_id { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public string school_type { get; set; }
        public string terminal_model { get; set; }
        public string tag_number { get; set; }
        public string serial_number { get; set; }
        public string hardware_status { get; set; }
        public string total_amount { get; set; }
        public int total_transaction { get; set; }
    }

    public class ProductCategoryProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ProductCategory> Data { get; set; }
    }

    public class ProductCategory
    {
        public int category_id { get; set; }
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string category_name { get; set; }
        public string category_description { get; set; }
        public string total_product { get; set; }
    }

    public class ProductDetailProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ProductDetail> Data { get; set; }
    }

    public class ProductDetail
    {
        public int product_id { get; set; }
        public int category_id { get; set; }
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string category_name { get; set; }
        public string product_name { get; set; }
        public string product_sku { get; set; }
        public string product_description { get; set; }
        public string photo_url { get; set; }
        public string file_name { get; set; }
        public string product_photo_url { get; set; }
        public string unit_price { get; set; }
        public string str_unit_price { get; set; }
        public string cost_price { get; set; }
        public string discount_price { get; set; }
        public string text_color { get; set; }
        public string background_color { get; set; }
        public string product_weight { get; set; }
        public string special_flag { get; set; }
        public string available_day { get; set; }
        public string product_ingredient { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
    }

    public class ProductNutritionProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ProductNutrition> Data { get; set; }
    }

    public class ProductNutrition
    {
        public int info_id { get; set; }
        public int product_id { get; set; }
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string nutrition_name { get; set; }
        public string per_gram { get; set; }
        public string per_serving { get; set; }
    }

    public class StudentMonthlyAttendanceProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentMonthlyAttendance> Data { get; set; }
    }

    public class StudentMonthlyAttendance
    {
        public int report_id { get; set; }
        public DateTime entry_date { get; set; }
        public int attendance_id { get; set; }
        public string attendance { get; set; }
        public string attendance_bm { get; set; }
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
    }

    public class ClubMemberMonthlyAttendanceProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClubMemberMonthlyAttendance> Data { get; set; }
    }

    public class ClubMemberMonthlyAttendance
    {
        public int report_id { get; set; }
        public DateTime entry_date { get; set; }
        public int attendance_id { get; set; }
        public string attendance { get; set; }
        public string attendance_bm { get; set; }
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
    }

    public class ClubMemberDailyAttendanceProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClubMemberDailyAttendance> Data { get; set; }
    }

    public class ClubMemberDailyAttendance
    {
        public int report_id { get; set; }
        public DateTime entry_date { get; set; }
        public int attendance_id { get; set; }
        public string attendance { get; set; }
        public string attendance_bm { get; set; }
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
    }

    public class ClassDailyAttendanceSummaryProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClassDailyAttendanceSummary> Data { get; set; }
    }

    public class ClassDailyAttendanceSummary
    {
        public int total_absent { get; set; }
        public int total_present { get; set; }
        public int total_late_in { get; set; }
        public int total_half_day { get; set; }
        public int total_attendance { get; set; }
        public int total_student { get; set; }
    }

    public class ClubDailyAttendanceSummaryProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClubDailyAttendanceSummary> Data { get; set; }
    }

    public class ClubDailyAttendanceSummary
    {
        public int total_absent { get; set; }
        public int total_present { get; set; }
        public int total_late_in { get; set; }
        public int total_half_day { get; set; }
        public int total_attendance { get; set; }
        public int total_member { get; set; }
    }

    public class StaffDailyAttendanceSummaryProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffDailyAttendanceSummary> Data { get; set; }
    }

    public class StaffDailyAttendanceSummary
    {
        public int total_absent { get; set; }
        public int total_present { get; set; }
        public int total_late_in { get; set; }
        public int total_half_day { get; set; }
        public int total_attendance { get; set; }
        public int total_staff { get; set; }
    }

    public class ClassDailyAttendancePercentageProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClassDailyAttendancePercentage> Data { get; set; }
    }

    public class ClassDailyAttendancePercentage
    {
        public int school_id { get; set; }
        public int class_id { get; set; }
        public DateTime entry_date { get; set; }
        public int total_student { get; set; }
        public int total_attendance { get; set; }
        public decimal total_percentage { get; set; }
    }

    public class ClubDailyAttendancePercentageProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClubDailyAttendancePercentage> Data { get; set; }
    }

    public class ClubDailyAttendancePercentage
    {
        public int school_id { get; set; }
        public int class_id { get; set; }
        public DateTime entry_date { get; set; }
        public int total_member { get; set; }
        public int total_attendance { get; set; }
        public decimal total_percentage { get; set; }
    }

    public class ClassDailyAttendanceProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClassDailyAttendance> Data { get; set; }
    }

    public class ClassDailyAttendance
    {
        public int report_id { get; set; }
        public int student_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public DateTime entry_date { get; set; }
        public int attendance_id { get; set; }
        public string attendance { get; set; }
        public string attendance_bm { get; set; }
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
        public bool dot_visible { get; set; }
    }

    public class ClubDailyAttendanceProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ClubDailyAttendance> Data { get; set; }
    }

    public class ClubDailyAttendance
    {
        public int report_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public DateTime entry_date { get; set; }
        public int user_role_id { get; set; }
        public string user_role { get; set; }
        public int attendance_id { get; set; }
        public string attendance { get; set; }
        public string attendance_bm { get; set; }
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
        public bool dot_visible { get; set; }
    }
    public class SchoolDailyAttendanceProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SchoolDailyAttendance> Data { get; set; }
    }

    public class SchoolDailyAttendance
    {
        public int report_id { get; set; }
        public int staff_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public DateTime entry_date { get; set; }
        public DateTime touch_in_at { get; set; }
        public DateTime touch_out_at { get; set; }
        public int attendance_id { get; set; }
        public string attendance { get; set; }
        public string attendance_bm { get; set; }
        public int reason_id { get; set; }
        public string reason_for_absent { get; set; }
        public string reason_for_absent_bm { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
        public bool dot_visible { get; set; }
    }
    public class ShoppingCartProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ShoppingCart> Data { get; set; }
    }
    public class ShoppingCart
    {
        public int cart_id { get; set; }
        public int profile_id { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public string full_name { get; set; }
        public int merchant_id { get; set; }
        public string company_name { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int recipient_id { get; set; }
        public int recipient_role_id { get; set; }
        public int user_role_id { get; set; }
        public string recipient_role { get; set; }
        public string recipient_name { get; set; }
        public DateTime pickup_date { get; set; }
        public int product_id { get; set; }
        public string product_photo_url { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public string str_unit_price { get; set; }
        public decimal unit_price { get; set; }
        public int product_qty { get; set; }
        public decimal sub_total_amount { get; set; }
        public int order_status_id { get; set; }
        public string order_status { get; set; }
        public string order_status_bm { get; set; }
        public bool is_check { get; set; }
    }

    public class OrderCart 
    {
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public string total_order { get; set; }
        public string order_status { get; set; }
        public string order_status_bm { get; set; }
    }
    public class OrderHistoryGroupProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<OrderHistoryGroup> Data { get; set; }
    }
    public class OrderHistoryGroup
    {
        public DateTime pickup_date { get; set; }
    }

    public class SettlementReportGroupProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SettlementReportGroup> Data { get; set; }
    }
    public class SettlementReportGroup
    {
        public DateTime receipt_date { get; set; }
    }
    public class OrderHistoryProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<OrderHistory> Data { get; set; }
    }
    public class OrderHistory
    {
        public int total_order { get; set; }
        public decimal total_amount { get; set; }
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public DateTime pickup_date { get; set; }
        public DateTime pickup_time { get; set; }
        public int service_method_id { get; set; }
        public string delivery_location { get; set; }
        public string order_id { get; set; }
        public int order_status_id { get; set; }
        public string order_status { get; set; }
        public string order_status_bm { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
    }
    public class SettlementReportProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<SettlementReport> Data { get; set; }
    }
    public class SettlementReport
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public DateTime receipt_date { get; set; }
        public string total_amount { get; set; }
        public string settlement_amount { get; set; }
        public string fee_amount { get; set; }
        public string net_amount { get; set; }
        public int sales_method_id { get; set; }
        public string sales_method { get; set; }
        public string sales_method_bm { get; set; }
        public int status_id { get; set; }
        public string status { get; set; }
        public string status_bm { get; set; }
        public DateTime payment_date { get; set; }
        public string status_color { get; set; }
    }
    public class StudentOrderHistoryProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentOrderHistory> Data { get; set; }
    }
    public class StudentOrderHistory
    {
        public DateTime pickup_date { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_photo_url { get; set; }
        public decimal unit_price { get; set; }
        public int product_qty { get; set; }
        public decimal total_amount { get; set; }
        public string str_total_amount { get; set; }
        public int recipient_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public string photo_url_student { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
    }

    public class ProductOrderHistoryProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ProductOrderHistory> Data { get; set; }
    }
    public class ProductOrderHistory
    {
        public DateTime pickup_date { get; set; }
        public int product_id { get; set; }
        public string product_total { get; set; }
        public string product_name { get; set; }
        public string product_photo_url { get; set; }
        public decimal unit_price { get; set; }
        public int product_qty { get; set; }
        public decimal total_amount { get; set; }
        public string str_total_amount { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
    }

    public class StudentOutingProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentOuting> Data { get; set; }
    }

    public class StudentOuting
    {
        public int outing_id { get; set; }
        public int student_id { get; set; }
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public string photo_url_student { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int outing_type_id { get; set; }
        public string outing_type { get; set; }
        public DateTime check_out_date { get; set; }
        public DateTime check_in_date { get; set; }
        public string outing_reason { get; set; }
        public int outing_status_id { get; set; }
        public string outing_status { get; set; }
        public int request_by_id { get; set; }
        public string request_by { get; set; }
        public int request_by_user_role_id { get; set; }
        public string request_by_user_role { get; set; }
        public int approve_by_id { get; set; }
        public string approve_by { get; set; }
        public DateTime approve_at { get; set; }
        public string approve_comment { get; set; }
        public DateTime check_out_at { get; set; }
        public DateTime check_in_at { get; set; }
        public string outing_date { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
        public bool is_check { get; set; }
        public string img_checkbox { get; set; }
    }
    public class StudentOutingGroupProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StudentOutingGroup> Data { get; set; }
    }

    public class StudentOutingGroup
    {
        public DateTime outing_month { get; set; }
    }
    public class StaffOutingMonthProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<StaffOutingMonth> Data { get; set; }
    }
    public class StaffOutingMonth
    {
        public int school_id { get; set; }
        public string school_name { get; set; }
        public int outing_type_id { get; set; }
        public string outing_type { get; set; }
        public int outing_status_id { get; set; }
        public string outing_status { get; set; }
        public string outing_application { get; set; }
        public DateTime outing_month { get; set; }
    }

    public class OrderHistoryMasterProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<OrderHistoryMaster> Data { get; set; }
    }
    public class OrderHistoryMaster
    {
        public int order_id { get; set; }
        public int profile_id { get; set; }
        public int wallet_id { get; set; }
        public string wallet_number { get; set; }
        public int user_role_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public string reference_number { get; set; }
        public string transaction_method { get; set; }
        public string transaction_type { get; set; }
        public string total_amount { get; set; }
        public string amount_color { get; set; }
        public decimal sub_total_amount { get; set; }
        public int order_status_id { get; set; }
        public string order_status { get; set; }
        public string order_status_bm { get; set; }
        public string status_color { get; set; }
        public string payment_method { get; set; }
        public string payment_method_bm { get; set; }
        public string create_at { get; set; }

    }

    public class MerchantSalesProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MerchantSales> Data { get; set; }
    }

    public class MerchantSales
    {
        public int merchant_id { get; set; }
        public string receipt_date { get; set; }
        public string total_amount { get; set; }
    }

    public class MerchantSalesMethodProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MerchantSalesMethod> Data { get; set; }
    }

    public class MerchantSalesMethod
    {
        public int merchant_id { get; set; }
        public int school_id { get; set; }
        public string school_name { get; set; }
        public string receipt_date { get; set; }
        public string total_amount { get; set; }
        public string sales_method { get; set; }
        public string sales_method_bm { get; set; }
    }

    public class ContactProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Contact> Data { get; set; }
    }

    public class Contact
    {
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
        public string user_role { get; set; }
        public string user_role_bm { get; set; }
    }

    public class ProblemTypeProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ProblemType> Data { get; set; }
    }

    public class ProblemType
    {
        public int problem_type_id { get; set; }
        public string problem_type { get; set; }
        public string problem_type_bm { get; set; }
        public int search_id { get; set; }
        public string search_name { get; set; }
    }

    public class ChatHistoryProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<ChatHistory> Data { get; set; }
    }

    public class ChatHistory
    {
        public string channel_id { get; set; }
        public int channel_type_id { get; set; }
        public string channel_type { get; set; }
        public string channel_name { get; set; }
        public string channel_name_full { get; set; }
        public string channel_photo_url { get; set; }
        public int profile_id { get; set; }
        public int sender_id { get; set; }
        public string sender_name { get; set; }
        public string sender_photo_url { get; set; }
        public int receiver_id { get; set; }
        public string receiver_name { get; set; }
        public string receiver_photo_url { get; set; }
        public string last_message { get; set; }
        public DateTime sent_at { get; set; }
        public string time_message { get; set; }
        public int unread_count { get; set; }
        public bool count_visible { get; set; }
        public bool image_visible { get; set; }
        public bool initial_visible { get; set; }
    }

    public class UserNotify
    {
        public int notify_id { get; set; }
        public string notify_subject { get; set; }
        public string notify_message { get; set; }
        public string notify_message_full { get; set; }
        public string notify_photo_url { get; set; }
        public string notify_link { get; set; }
        public string notify_link_text { get; set; }
        public string notify_link_param { get; set; }
        public string read_flag { get; set; }
        public string create_at { get; set; }
        public string message_color { get; set; }
        public string subject_color { get; set; }
    }

    public class UserNotifyProperty
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<UserNotify> Data { get; set; }
    }

    public class PlatformVersionProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<PlatformVersion> Data { get; set; }
    }

    public class PlatformVersion
    {
        public string platform_name { get; set; }
        public string version_number { get; set; }
        public string build_number { get; set; }
        public string release { get; set; }
    }

    public class AccountCSInfoProperty
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<AccountCSInfo> Data { get; set; }
    }

    public class AccountCSInfo
    {
        public string full_name { get; set; }
        public string email { get; set; }
        public string school_name { get; set; }
        public string coordinate { get; set; }
    }
}
