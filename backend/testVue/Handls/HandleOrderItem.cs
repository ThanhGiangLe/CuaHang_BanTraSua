
using testVue.Configs;
using testVue.Datas;

namespace testVue.Handls
{
    public class HandleOrderItem : IHandleOrderItem
    {
        private readonly AppDbContext _appDbContext;
        private IHandleMaterials _iHandleMaterials;
        private MaterialIdConfigs _materialIdConfigs;
        private MaterialConfigs _materialConfigs;
        public HandleOrderItem(IHandleMaterials iHandleMaterials, MaterialIdConfigs materialIdConfigs, AppDbContext appDbContext, MaterialConfigs materialConfigs)
        {
            _iHandleMaterials = iHandleMaterials;
            _materialIdConfigs = materialIdConfigs;
            _appDbContext = appDbContext;
            _materialConfigs = materialConfigs;
        }

        public async Task<bool> UpdateQuantityMaterialId_BanhCuon(int quantity)
        {
            bool results = true;

            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhCuon_BotBanhCuon(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhCuon_HanhPhi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhCuon_MocNhi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhCuon_NuocMam(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhCuon_ThitBam(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_BanhMiTrung(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhMiTrung_BanhMi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhMiTrung_Bo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhMiTrung_DuaLeo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhMiTrung_Mayonnaise(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhMiTrung_TrungGa(quantity);
            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_BanhXeo(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhXeo_BotBanhXeo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhXeo_GiaDo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhXeo_HanhLa(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhXeo_NuocCotDua(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhXeo_ThitBaChi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhXeo_TomTuoi(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_BapRangBo(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BapRangBo_Bap(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BapRangBo_Bo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BapRangBo_Duong(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_ChaoGa(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaoGa_GaoTe(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaoGa_HanhLa(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaoGa_NamRom(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaoGa_NuocMam(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaoGa_RauSong(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaoGa_ThitGa(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaoGa_Tieu(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_Combo_BanhNgot(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ComboBanhNgot_BongLan(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ComboBanhNgot_Croissant(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ComboBanhNgot_XuKem(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_Combo_CaVienChien_Cocacola(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_XienBan_Cocacola(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_XienBan_Duong(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_XienBan_NuocMam(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_XienBan_ToiOt(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_XienBan_XienBan(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_XienBan_XucXich(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_ComGa(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterialRice(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterialChicken(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterialFishSauce(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterialGarlic(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterialVegetable(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterialCucumber(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_ComGaXoiMo(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ComGaXoiMo_DuaLeo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ComGaXoiMo_Ga(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ComGaXoiMo_Gao(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ComGaXoiMo_NuocMam(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ComGaXoiMo_RauSong(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ComGaXoiMo_ToiOt(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_GaNuongComLam_BanhBaoChien(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GaComLam_BanhBaoChien(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GaComLam_DuaLeo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GaComLam_GaNguyenCon(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GaComLam_GaoTe(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GaComLam_RauSong(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_GaRan(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GaRan_Ga(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GaRan_BotChienGion(quantity);
            results &=results & await _iHandleMaterials.UpdateQuantityMaterial_GaRan_DauAn(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GaRan_MuoiTieu(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GaRan_Toi(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_GoiCuon(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GoiCuon_BanhTrang(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GoiCuon_BunTuoi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GoiCuon_RauSong(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GoiCuon_ThitBaChi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GoiCuon_TomTuoi(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_HuTieuNamVang(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_HuTieu_GanHeo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_HuTieu_GiaDo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_HuTieu_HanhPhi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_HuTieu_NuocDung(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_HuTieu_SoiHuTieu(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_HuTieu_ThitBam(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_HuTieu_TomTuoi(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_KemDua(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_KemDua_DuaSoi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_KemDua_Duong(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_KemDua_NuocCotDua(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_KemTraiCay(int quantity)
        {
            var results = await _iHandleMaterials.UpdateQuantityMaterial_SuaTuoi(quantity);
            var result1 = await _iHandleMaterials.UpdateQuantityMaterial_SuaDac(quantity);
            var result2 = await _iHandleMaterials.UpdateQuantityMaterial_TraiCayTuoi(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_KhoaiTayChien(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_KhoaiTayChien_DauAn(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_KhoaiTayChien_KhaoiTay(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_KhoaiTayChien_TuongOt(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_MiXaoBo(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MiXaoBo_CaiNgot(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MiXaoBo_HanhLa(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MiXaoBo_HanhTay(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MiXaoBo_MiTrung(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MiXaoBo_ThitBo(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_MiXaoOcMongTay(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MiXaoOcMongTay_DauHao(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MiXaoOcMongTay_MiTrung(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MiXaoOcMongTay_OcMongtay(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MiXaoOcMongTay_RauRam(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MiXaoOcMongTay_Toi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MiXaoOcMongTay_XiDau(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_PhoBo(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhPho(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ThitBo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GiaViPho(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_GiaDo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterialVegetable(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChanhTuoi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ToiOt(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_PhoMaiQue(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_PhoMaiQue_BotChienXu(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_PhoMaiQue_DauAn(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_PhoMaiQue_PhomaiMozzaarella(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_PizzaPhoMai(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_DeDanhPizza(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_SotCaChua(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_PhoMaiMozzarella(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_XucXich(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ThitNguoi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_OtChuong(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_HanhTay(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_Sandwich(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhMi_BanhMi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhMi_Bow(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhMi_DoChua(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhMi_DuaLeo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhMi_Mayonnaise(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhMi_PaTeChaLua(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BanhMi_ThitNguoi(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_SinhToXoai(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_SinhToXoai_DaVien(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_SinhToXoai_SuaDac(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_SinhToXoai_SuaTuoi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_SinhToXoai_XoaiChin(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_SupCua(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_SupCua_BotNang(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_SupCua_HatBap(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_SupCua_NamDongCo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_SupCua_ThitCua(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_SupCua_TrungGa(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_TraChanh(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_TraChanh_ChanhTuoi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_TraChanh_DaVien(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_TraChanh_Duong(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_TraChanh_TraXanh(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_TraSua(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_TraDen(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_SuaDac(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_TranChau(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_NuocDuong(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_DaVien(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_XucXichChien(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_XucXichChien_DauAn(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_XucXichChien_TuongOt(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_XucXichChien_XucXich(quantity);

            return results;
        }

        public async Task<bool> UpdateQuantityMaterialId_TranChauDen_MonThem(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TranChauDen && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TranChauDen && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TranChauDen;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TranChauDen;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialId_TranChauTrang_MonThem(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TranChauTrang && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TranChauTrang && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TranChauTrang;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TranChauTrang;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialId_ThachCaPhe_MonThem(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThachCaPhe && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThachCaPhe && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ThachCaPhe;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ThachCaPhe;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialId_PuddingTrung_MonThem(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Pudding && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Pudding && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PuddingTrung;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PuddingTrung;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialId_RauCauDua_MonThem(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauCauDua && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauCauDua && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.RauCauDua;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.RauCauDua;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialId_BanhPhoThem_MonThem(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhPhoThem && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhPhoThem && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhPhoThem;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhPhoThem;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialId_TrungTran_MonThem(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TrungGa && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TrungGa && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TrungTran;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TrungTran;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialId_BoVienThem_MonThem(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BoVien && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BoVien && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BoVienThem;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BoVienThem;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialId_GioHeo_MonThem(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GioHeo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GioHeo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GioHeo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GioHeo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialId_HanhPhi_MonThem(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhPhi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhPhi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.HanhPhi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.HanhPhi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialId_RauSong_MonThem(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauSong && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauSong && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.RauSong;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.RauSong;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterialId_ChaGio_MonThem(int quantity)
        {
            bool results = true; // Mặc định là true để tất cả các hàm con được gọi

            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaGio_BanhTrang(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaGio_CaRot(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaGio_HanhTim(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaGio_Mien(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaGio_NamMeo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaGio_ThitBam(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_ChaGio_TrungGa(quantity);

            return results;
        }
        public async Task<bool> UpdateQuantityMaterialId_MucChien_MonThem(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MucChien_BotChienGion(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MucChien_DauAn(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MucChien_MucTuoi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_MucChien_TrungGa(quantity);

            return results;
        }
        public async Task<bool> UpdateQuantityMaterialId_TomChien_MonThem(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_TomChien_BotChienXu(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_TomChien_DauAn(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_TomChien_TomTuoi(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_TomChien_TrungGa(quantity);
            return results;
        }
        public async Task<bool> UpdateQuantityMaterialId_NacHeoXao_MonThem(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_NacHeoXao_DauAn(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_NacHeoXao_DauHao(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_NacHeoXao_HanhTay(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_NacHeoXao_ThitHeo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_NacHeoXao_Toi(quantity);

            return results;
        }
        public async Task<bool> UpdateQuantityMaterialId_RauXao_MonThem(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_RauXao_CaiNgot(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_RauXao_DauAn(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_RauXao_DauHao(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_RauXao_MuoiTieu(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_RauXao_Toi(quantity);

            return results;
        }
        public async Task<bool> UpdateQuantityMaterialId_BoXao_MonThem(int quantity)
        {
            bool results = true;
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BoXao_DauAn(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BoXao_DauHao(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BoXao_HanhTay(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BoXao_ThitBo(quantity);
            results &= await _iHandleMaterials.UpdateQuantityMaterial_BoXao_Toi(quantity);

            return results;
        }
    }
}
