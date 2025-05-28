namespace testVue.Handls
{
    public interface IHandleMaterials
    {
        // Nguyên liệu dành cho món cơm gà chiên mắm tỏi
        public Task<bool> UpdateQuantityMaterialRice(int quantity);
        public Task<bool> UpdateQuantityMaterialChicken(int quantity);
        public Task<bool> UpdateQuantityMaterialGarlic(int quantity);
        public Task<bool> UpdateQuantityMaterialFishSauce(int quantity);
        public Task<bool> UpdateQuantityMaterialVegetable(int quantity);
        public Task<bool> UpdateQuantityMaterialCucumber(int quantity);

        // Nguyên liệu dành cho món Phở bò
        public Task<bool> UpdateQuantityMaterial_BanhPho(int quantity);
        public Task<bool> UpdateQuantityMaterial_ThitBo(int quantity);
        public Task<bool> UpdateQuantityMaterial_GiaViPho(int quantity);
        public Task<bool> UpdateQuantityMaterial_GiaDo(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChanhTuoi(int quantity);
        public Task<bool> UpdateQuantityMaterial_ToiOt(int quantity);

        // Nguyên liệu dành cho món bánh pizza phô mai
        public Task<bool> UpdateQuantityMaterial_DeDanhPizza(int quantity);
        public Task<bool> UpdateQuantityMaterial_SotCaChua(int quantity);
        public Task<bool> UpdateQuantityMaterial_PhoMaiMozzarella(int quantity);
        public Task<bool> UpdateQuantityMaterial_XucXich(int quantity);
        public Task<bool> UpdateQuantityMaterial_ThitNguoi(int quantity);
        public Task<bool> UpdateQuantityMaterial_OtChuong(int quantity);
        public Task<bool> UpdateQuantityMaterial_HanhTay(int quantity);

        // Nguyên liệu dành cho món Kem trái cây
        public Task<bool> UpdateQuantityMaterial_SuaTuoi(int quantity);
        public Task<bool> UpdateQuantityMaterial_SuaDac(int quantity);
        public Task<bool> UpdateQuantityMaterial_TraiCayTuoi(int quantity);

        // Nguyên liệu dành cho món Trà sữa
        public Task<bool> UpdateQuantityMaterial_TraDen(int quantity);
        public Task<bool> UpdateQuantityMaterial_TranChau(int quantity);
        public Task<bool> UpdateQuantityMaterial_NuocDuong(int quantity);
        public Task<bool> UpdateQuantityMaterial_DaVien(int quantity);

        // Nguyên liệu dành cho món Bánh mì
        public Task<bool> UpdateQuantityMaterial_BanhMi_BanhMi(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhMi_ThitNguoi(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhMi_PaTeChaLua(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhMi_DuaLeo(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhMi_DoChua(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhMi_Bow(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhMi_Mayonnaise(int quantity);

        // Nguyên liệu dành cho món Gà rán
        public Task<bool> UpdateQuantityMaterial_GaRan_Ga(int quantity);
        public Task<bool> UpdateQuantityMaterial_GaRan_BotChienGion(int quantity);
        public Task<bool> UpdateQuantityMaterial_GaRan_DauAn(int quantity);
        public Task<bool> UpdateQuantityMaterial_GaRan_MuoiTieu(int quantity);
        public Task<bool> UpdateQuantityMaterial_GaRan_Toi(int quantity);

        // Nguyên liệu dành cho món Bánh xèo
        public Task<bool> UpdateQuantityMaterial_BanhXeo_BotBanhXeo(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhXeo_NuocCotDua(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhXeo_TomTuoi(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhXeo_ThitBaChi(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhXeo_GiaDo(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhXeo_HanhLa(int quantity);

        // Nguyên liệu dành cho món Cháo gà
        public Task<bool> UpdateQuantityMaterial_ChaoGa_GaoTe(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChaoGa_ThitGa(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChaoGa_HanhLa(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChaoGa_RauSong(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChaoGa_NamRom(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChaoGa_Tieu(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChaoGa_NuocMam(int quantity);

        // Nguyên liệu dành cho món sinh tố xoài
        public Task<bool> UpdateQuantityMaterial_SinhToXoai_XoaiChin(int quantity);
        public Task<bool> UpdateQuantityMaterial_SinhToXoai_SuaDac(int quantity);
        public Task<bool> UpdateQuantityMaterial_SinhToXoai_SuaTuoi(int quantity);
        public Task<bool> UpdateQuantityMaterial_SinhToXoai_DaVien(int quantity);

        // Nguyên liệu dành cho món Mì xào bò
        public Task<bool> UpdateQuantityMaterial_MiXaoBo_MiTrung(int quantity);
        public Task<bool> UpdateQuantityMaterial_MiXaoBo_ThitBo(int quantity);
        public Task<bool> UpdateQuantityMaterial_MiXaoBo_CaiNgot(int quantity);
        public Task<bool> UpdateQuantityMaterial_MiXaoBo_HanhTay(int quantity);
        public Task<bool> UpdateQuantityMaterial_MiXaoBo_HanhLa(int quantity);

        // Nguyên liệu dành cho món gỏi cuốn
        public Task<bool> UpdateQuantityMaterial_GoiCuon_BanhTrang(int quantity);
        public Task<bool> UpdateQuantityMaterial_GoiCuon_ThitBaChi(int quantity);
        public Task<bool> UpdateQuantityMaterial_GoiCuon_TomTuoi(int quantity);
        public Task<bool> UpdateQuantityMaterial_GoiCuon_RauSong(int quantity);
        public Task<bool> UpdateQuantityMaterial_GoiCuon_BunTuoi(int quantity);

        // Nguyên liệu dành cho món trà chanh
        public Task<bool> UpdateQuantityMaterial_TraChanh_TraXanh(int quantity);
        public Task<bool> UpdateQuantityMaterial_TraChanh_ChanhTuoi(int quantity);
        public Task<bool> UpdateQuantityMaterial_TraChanh_Duong(int quantity);
        public Task<bool> UpdateQuantityMaterial_TraChanh_DaVien(int quantity);

        // Nguyên liệu dành cho món kem dừa
        public Task<bool> UpdateQuantityMaterial_KemDua_NuocCotDua(int quantity);
        public Task<bool> UpdateQuantityMaterial_KemDua_Duong(int quantity);
        public Task<bool> UpdateQuantityMaterial_KemDua_DuaSoi(int quantity);

        // Nguyên liệu dành cho món Bánh mì trứng
        public Task<bool> UpdateQuantityMaterial_BanhMiTrung_BanhMi(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhMiTrung_TrungGa(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhMiTrung_Bo(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhMiTrung_Mayonnaise(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhMiTrung_DuaLeo(int quantity);

        // Nguyên liệu dành cho món Hủ tiếu Nam vang
        public Task<bool> UpdateQuantityMaterial_HuTieu_SoiHuTieu(int quantity);
        public Task<bool> UpdateQuantityMaterial_HuTieu_ThitBam(int quantity);
        public Task<bool> UpdateQuantityMaterial_HuTieu_TomTuoi(int quantity);
        public Task<bool> UpdateQuantityMaterial_HuTieu_GanHeo(int quantity);
        public Task<bool> UpdateQuantityMaterial_HuTieu_NuocDung(int quantity);
        public Task<bool> UpdateQuantityMaterial_HuTieu_HanhPhi(int quantity);
        public Task<bool> UpdateQuantityMaterial_HuTieu_GiaDo(int quantity);

        // Nguyên liệu dành cho món Súp cua
        public Task<bool> UpdateQuantityMaterial_SupCua_ThitCua(int quantity);
        public Task<bool> UpdateQuantityMaterial_SupCua_NamDongCo(int quantity);
        public Task<bool> UpdateQuantityMaterial_SupCua_HatBap(int quantity);
        public Task<bool> UpdateQuantityMaterial_SupCua_BotNang(int quantity);
        public Task<bool> UpdateQuantityMaterial_SupCua_TrungGa(int quantity);

        // Nguyên liệu dành cho món Xúc xích chiên
        public Task<bool> UpdateQuantityMaterial_XucXichChien_XucXich(int quantity);
        public Task<bool> UpdateQuantityMaterial_XucXichChien_DauAn(int quantity);
        public Task<bool> UpdateQuantityMaterial_XucXichChien_TuongOt(int quantity);

        // Nguyên liệu dành cho món Khoai tây chiên
        public Task<bool> UpdateQuantityMaterial_KhoaiTayChien_KhaoiTay(int quantity);
        public Task<bool> UpdateQuantityMaterial_KhoaiTayChien_DauAn(int quantity);
        public Task<bool> UpdateQuantityMaterial_KhoaiTayChien_TuongOt(int quantity);

        // Nguyên liệu dành cho món Bắp rang bơ
        public Task<bool> UpdateQuantityMaterial_BapRangBo_Bap(int quantity);
        public Task<bool> UpdateQuantityMaterial_BapRangBo_Bo(int quantity);
        public Task<bool> UpdateQuantityMaterial_BapRangBo_Duong(int quantity);

        // Nguyên liệu dành cho món Phô mai que
        public Task<bool> UpdateQuantityMaterial_PhoMaiQue_PhomaiMozzaarella(int quantity);
        public Task<bool> UpdateQuantityMaterial_PhoMaiQue_BotChienXu(int quantity);
        public Task<bool> UpdateQuantityMaterial_PhoMaiQue_DauAn(int quantity);

        // Nguyên liệu dành cho Xiên bẩn
        public Task<bool> UpdateQuantityMaterial_XienBan_XucXich(int quantity);
        public Task<bool> UpdateQuantityMaterial_XienBan_XienBan(int quantity);
        public Task<bool> UpdateQuantityMaterial_XienBan_NuocMam(int quantity);
        public Task<bool> UpdateQuantityMaterial_XienBan_Duong(int quantity);
        public Task<bool> UpdateQuantityMaterial_XienBan_ToiOt(int quantity);
        public Task<bool> UpdateQuantityMaterial_XienBan_Cocacola(int quantity);

        // Nguyên liệu dành cho món cơm gà xối mở
        public Task<bool> UpdateQuantityMaterial_ComGaXoiMo_Gao(int quantity);
        public Task<bool> UpdateQuantityMaterial_ComGaXoiMo_Ga(int quantity);
        public Task<bool> UpdateQuantityMaterial_ComGaXoiMo_DuaLeo(int quantity);
        public Task<bool> UpdateQuantityMaterial_ComGaXoiMo_RauSong(int quantity);
        public Task<bool> UpdateQuantityMaterial_ComGaXoiMo_NuocMam(int quantity);
        public Task<bool> UpdateQuantityMaterial_ComGaXoiMo_ToiOt(int quantity);

        // Nguyên liệu dành cho món Mì xào ốc móng tay
        public Task<bool> UpdateQuantityMaterial_MiXaoOcMongTay_MiTrung(int quantity);
        public Task<bool> UpdateQuantityMaterial_MiXaoOcMongTay_OcMongtay(int quantity);
        public Task<bool> UpdateQuantityMaterial_MiXaoOcMongTay_RauRam(int quantity);
        public Task<bool> UpdateQuantityMaterial_MiXaoOcMongTay_Toi(int quantity);
        public Task<bool> UpdateQuantityMaterial_MiXaoOcMongTay_DauHao(int quantity);
        public Task<bool> UpdateQuantityMaterial_MiXaoOcMongTay_XiDau(int quantity);

        // Nguyên liệu dành cho Commbo bánh ngọt
        public Task<bool> UpdateQuantityMaterial_ComboBanhNgot_BongLan(int quantity);
        public Task<bool> UpdateQuantityMaterial_ComboBanhNgot_XuKem(int quantity);
        public Task<bool> UpdateQuantityMaterial_ComboBanhNgot_Croissant(int quantity);

        // Nguyên liệu dành cho Bánh cuốn
        public Task<bool> UpdateQuantityMaterial_BanhCuon_BotBanhCuon(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhCuon_ThitBam(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhCuon_MocNhi(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhCuon_HanhPhi(int quantity);
        public Task<bool> UpdateQuantityMaterial_BanhCuon_NuocMam(int quantity);

        // Nguyên liệu dành cho Gà nướng cơm lam
        public Task<bool> UpdateQuantityMaterial_GaComLam_GaNguyenCon(int quantity);
        public Task<bool> UpdateQuantityMaterial_GaComLam_GaoTe(int quantity);
        public Task<bool> UpdateQuantityMaterial_GaComLam_BanhBaoChien(int quantity);
        public Task<bool> UpdateQuantityMaterial_GaComLam_RauSong(int quantity);
        public Task<bool> UpdateQuantityMaterial_GaComLam_DuaLeo(int quantity);

        public Task<bool> UpdateQuantityMaterial_ChaGio_BanhTrang(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChaGio_ThitBam(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChaGio_CaRot(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChaGio_Mien(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChaGio_NamMeo(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChaGio_HanhTim(int quantity);
        public Task<bool> UpdateQuantityMaterial_ChaGio_TrungGa(int quantity);


        public Task<bool> UpdateQuantityMaterial_MucChien_MucTuoi(int quantity);
        public Task<bool> UpdateQuantityMaterial_MucChien_BotChienGion(int quantity);
        public Task<bool> UpdateQuantityMaterial_MucChien_TrungGa(int quantity);
        public Task<bool> UpdateQuantityMaterial_MucChien_DauAn(int quantity);

        public Task<bool> UpdateQuantityMaterial_TomChien_TomTuoi(int quantity);
        public Task<bool> UpdateQuantityMaterial_TomChien_BotChienXu(int quantity);
        public Task<bool> UpdateQuantityMaterial_TomChien_TrungGa(int quantity);
        public Task<bool> UpdateQuantityMaterial_TomChien_DauAn(int quantity);

        public Task<bool> UpdateQuantityMaterial_NacHeoXao_ThitHeo(int quantity);
        public Task<bool> UpdateQuantityMaterial_NacHeoXao_HanhTay(int quantity);
        public Task<bool> UpdateQuantityMaterial_NacHeoXao_Toi(int quantity);
        public Task<bool> UpdateQuantityMaterial_NacHeoXao_DauHao(int quantity);
        public Task<bool> UpdateQuantityMaterial_NacHeoXao_DauAn(int quantity);

        public Task<bool> UpdateQuantityMaterial_RauXao_CaiNgot(int quantity);
        public Task<bool> UpdateQuantityMaterial_RauXao_Toi(int quantity);
        public Task<bool> UpdateQuantityMaterial_RauXao_DauAn(int quantity);
        public Task<bool> UpdateQuantityMaterial_RauXao_MuoiTieu(int quantity);
        public Task<bool> UpdateQuantityMaterial_RauXao_DauHao(int quantity);

        public Task<bool> UpdateQuantityMaterial_BoXao_ThitBo(int quantity);
        public Task<bool> UpdateQuantityMaterial_BoXao_HanhTay(int quantity);
        public Task<bool> UpdateQuantityMaterial_BoXao_Toi(int quantity);
        public Task<bool> UpdateQuantityMaterial_BoXao_DauHao(int quantity);
        public Task<bool> UpdateQuantityMaterial_BoXao_DauAn(int quantity);

    }
}
