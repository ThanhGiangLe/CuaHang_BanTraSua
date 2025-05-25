using testVue.Configs;
using testVue.Datas;

namespace testVue.Handls
{
    public class HandleMaterials : IHandleMaterials
    {
        private readonly AppDbContext _appDbContext;
        private MaterialConfigs _materialConfigs;
        private MaterialIdConfigs _materialIdConfigs;
        public HandleMaterials(AppDbContext appDbContext, MaterialConfigs materialConfigs, MaterialIdConfigs materialIdConfigs)
        {
            _appDbContext = appDbContext;
            _materialConfigs = materialConfigs;
            _materialIdConfigs = materialIdConfigs;
        }

        public async Task<bool> UpdateQuantityMaterialChicken(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Ga && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Ga && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComGa_Chicken;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComGa_Chicken;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialGarlic(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Toi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Toi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComGa_Garlic;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComGa_Garlic;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialFishSauce(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocMam && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocMam && item.Using == 1 && item.CurrentQuantity > 1);

            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComGa_FishSauce;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComGa_FishSauce;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);

            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialRice(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Gao && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Gao && item.Using == 1 && item.CurrentQuantity > 1);

            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComGa_Rice;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComGa_Rice;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);

            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialVegetable(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauSong && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauSong && item.Using == 1 && item.CurrentQuantity > 1);

            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComGa_Vegetable;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComGa_Vegetable;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);

            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterialCucumber(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DuaLeo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DuaLeo && item.Using == 1 && item.CurrentQuantity > 1);

            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComGa_Cucumber;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComGa_Cucumber;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);

            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_BanhPho(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhPho && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhPho && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PhoBo_BanhPho;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PhoBo_BanhPho;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);

            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ThitBo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBo && item.Using == 1 && item.CurrentQuantity > 1);

            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PhoBo_ThitBo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PhoBo_ThitBo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);

            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GiaViPho(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GiaViPho && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GiaViPho && item.Using == 1 && item.CurrentQuantity > 1);

            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PhoBo_GiaViPho;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PhoBo_GiaViPho;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);

            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GiaDo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GiaDo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GiaDo && item.Using == 1 && item.CurrentQuantity > 1);

            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PhoBo_GiaDo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PhoBo_GiaDo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);

            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChanhTuoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ChanhTuoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ChanhTuoi && item.Using == 1 && item.CurrentQuantity > 1);

            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PhoBo_ChanhTuoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PhoBo_ChanhTuoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);

            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ToiOt(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ToiOt && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ToiOt && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PhoBo_ToiOt;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PhoBo_ToiOt;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);

            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_DeDanhPizza(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DeBanhPizza && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DeBanhPizza && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PizzaPhoMai_DeDanhPizza;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PizzaPhoMai_DeDanhPizza;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_SotCaChua(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.SotCaChua && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.SotCaChua && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PizzaPhoMai_SotCaChua;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PizzaPhoMai_SotCaChua;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_PhoMaiMozzarella(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.PhoMaiMozzarella && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.PhoMaiMozzarella && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PizzaPhoMai_PhoMaiMozzarella;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PizzaPhoMai_SotCaChua;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_XucXich(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.XucXich && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.XucXich && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PizzaPhoMai_XucXich;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PizzaPhoMai_XucXich;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ThitNguoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitNguoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitNguoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PizzaPhoMai_ThitNguoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PizzaPhoMai_ThitNguoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_OtChuong(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.OtChuong && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.OtChuong && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PizzaPhoMai_OtChuong;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PizzaPhoMai_OtChuong;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_HanhTay(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhTya && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhTya && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PizzaPhoMai_HanhTay;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PizzaPhoMai_OtChuong;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_SuaTuoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.SuaTuoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.SuaTuoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.KemTraiCay_SuaTuoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.KemTraiCay_SuaTuoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_SuaDac(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.SuaDac && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.SuaTuoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.KemTraiCay_SuaDac;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.KemTraiCay_SuaDac;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_TraiCayTuoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TraiCayTuoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TraiCayTuoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.KemTraiCay_TraiCayTuoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.KemTraiCay_TraiCayTuoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_TraDen(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TraDen && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TraDen && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TraSua_TraDen;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TraSua_TraDen;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_TranChau(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TranChau && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TranChau && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TraSua_TranChau;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TraSua_TranChau;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_NuocDuong(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Duong && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Duong && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TraSua_NuocDuong;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TraSua_NuocDuong;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_DaVien(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DaVien && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DaVien && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TraSua_DaVien;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TraSua_DaVien;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_BanhMi_BanhMi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhMi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhMi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhMi_BanhMi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhMi_BanhMi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhMi_ThitNguoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitNguoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitNguoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhMi_ThitNguoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhMi_ThitNguoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhMi_PaTeChaLua(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.PateChaLua && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.PateChaLua && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhMi_PateChaLua;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhMi_PateChaLua;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhMi_DuaLeo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DuaLeo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DuaLeo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhMi_DuaLeo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhMi_DuaLeo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhMi_DoChua(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DoChua && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DoChua && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhMi_DoChua;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhMi_DoChua;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhMi_Bow(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Bow && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Bow && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhMi_Bow;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhMi_Bow;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhMi_Mayonnaise(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Mayonnaise && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Mayonnaise && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhMi_Mayonnaise;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhMi_Mayonnaise;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_GaRan_Ga(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Ga && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Ga && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GaRan_Ga;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GaRan_Ga;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GaRan_BotChienGion(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotChienGion && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotChienGion && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GaRan_BotChienGion;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GaRan_BotChienGion;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GaRan_DauAn(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GaRan_DauAn;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GaRan_DauAn;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GaRan_MuoiTieu(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MuoiTieu && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MuoiTieu && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GaRan_MuoiTieu;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GaRan_MuoiTieu;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GaRan_Toi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Toi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Toi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GaRan_Toi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GaRan_Toi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_BanhXeo_BotBanhXeo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotBanhXeo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotBanhXeo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhXeo_BotBanhXeo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhXeo_BotBanhXeo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhXeo_NuocCotDua(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocCotDua && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocCotDua && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhXeo_NuocCotDua;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhXeo_NuocCotDua;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhXeo_TomTuoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TomTuoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TomTuoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhXeo_TomTuoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhXeo_TomTuoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhXeo_ThitBaChi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBaChi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBaChi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhXeo_ThitBaChi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhXeo_ThitBaChi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhXeo_GiaDo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GiaDo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GiaDo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhXeo_GiaDo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhXeo_GiaDo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhXeo_HanhLa(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhLa && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhLa && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhXeo_HanhLa;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhXeo_HanhLa;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_ChaoGa_GaoTe(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GaoTe && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GaoTe && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaoGa_GaoTe;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaoGa_GaoTe;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChaoGa_ThitGa(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Ga && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Ga && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaoGa_ThitGa;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaoGa_ThitGa;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChaoGa_HanhLa(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhLa && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhLa && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaoGa_HanhLa;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaoGa_HanhLa;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChaoGa_RauSong(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauSong && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauSong && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaoGa_RauSong;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaoGa_RauSong;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChaoGa_NamRom(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NamRom && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NamRom && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaoGa_NamRom;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaoGa_NamRom;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChaoGa_Tieu(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MuoiTieu && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MuoiTieu && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaoGa_Tieu;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaoGa_Tieu;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChaoGa_NuocMam(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocMam && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocMam && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaoGa_NuocMam;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaoGa_NuocMam;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_SinhToXoai_XoaiChin(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.XoaiChin && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.XoaiChin && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.SinhToXoai_XoaiChin;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.SinhToXoai_XoaiChin;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_SinhToXoai_SuaDac(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.SuaDac && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.SuaDac && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.SinhToXoai_SuaDac;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.SinhToXoai_SuaDac;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_SinhToXoai_SuaTuoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.SuaTuoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.SuaTuoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.SinhToXoai_SuaTuoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.SinhToXoai_SuaTuoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_SinhToXoai_DaVien(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DaVien && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DaVien && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.SinhToXoai_DaVien;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.SinhToXoai_DaVien;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_MiXaoBo_MiTrung(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MiTrung && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MiTrung && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MiXaoBo_MiTrung;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MiXaoBo_MiTrung;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_MiXaoBo_ThitBo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MiXaoBo_ThitBo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MiXaoBo_ThitBo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_MiXaoBo_CaiNgot(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.CaiNgot && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.CaiNgot && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MiXaoBo_CaiNgot;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MiXaoBo_CaiNgot;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_MiXaoBo_HanhTay(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhTya && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhTya && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MiXaoBo_HanhTay;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MiXaoBo_HanhTay;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_MiXaoBo_HanhLa(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhLa && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhLa && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MiXaoBo_HanhLa;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MiXaoBo_HanhLa;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_GoiCuon_BanhTrang(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhTrang && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhTrang && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GoiCuon_BanhTrang;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GoiCuon_BanhTrang;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GoiCuon_ThitBaChi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBaChi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBaChi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GoiCuon_ThitBaChi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GoiCuon_ThitBaChi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GoiCuon_TomTuoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TomTuoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TomTuoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GoiCuon_TomTuoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GoiCuon_TomTuoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GoiCuon_RauSong(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauSong && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauSong && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GoiCuon_RauSong;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GoiCuon_RauSong;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GoiCuon_BunTuoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BunTuoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BunTuoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GoiCuon_BunTuoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GoiCuon_BunTuoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_TraChanh_TraXanh(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TraXanh && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TraXanh && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TraChanh_TraXanh;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TraChanh_TraXanh;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_TraChanh_ChanhTuoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ChanhTuoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ChanhTuoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TraChanh_ChanhTuoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TraChanh_ChanhTuoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_TraChanh_Duong(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Duong && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Duong && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TraChanh_Duong;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TraChanh_Duong;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_TraChanh_DaVien(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DaVien && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DaVien && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TraChanh_DaVien;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TraChanh_DaVien;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_KemDua_NuocCotDua(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocCotDua && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocCotDua && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.KemDua_NuocCotDua;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.KemDua_NuocCotDua;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_KemDua_Duong(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Duong && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Duong && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.KemDua_Duong;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.KemDua_Duong;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_KemDua_DuaSoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DuaNao && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DuaNao && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.KemDua_DuaSoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.KemDua_DuaSoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_BanhMiTrung_BanhMi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhMi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhMi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhMiTrung_BanhMi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhMiTrung_BanhMi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhMiTrung_TrungGa(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TrungGa && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TrungGa && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhMiTrung_TrungGa;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhMiTrung_TrungGa;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhMiTrung_Bo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Bow && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Bow && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhMiTrung_Bo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhMiTrung_Bo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhMiTrung_Mayonnaise(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Mayonnaise && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Mayonnaise && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhMiTrung_Mayonnaise;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhMiTrung_Mayonnaise;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhMiTrung_DuaLeo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DuaLeo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DuaLeo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhMiTrung_DuaLeo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhMiTrung_DuaLeo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_HuTieu_SoiHuTieu(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HuTieuKho && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HuTieuKho && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.HuTieu_SoiHuTieu;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.HuTieu_SoiHuTieu;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_HuTieu_ThitBam(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBam && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBam && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.HuTieu_ThitBam;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.HuTieu_ThitBam;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_HuTieu_TomTuoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TomTuoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TomTuoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.HuTieu_TomTuoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.HuTieu_TomTuoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_HuTieu_GanHeo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GanHeo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GanHeo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.HuTieu_GanHeo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.HuTieu_GanHeo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_HuTieu_NuocDung(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GiaViHuTieu && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GiaViHuTieu && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.HuTieu_NuocDung;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.HuTieu_NuocDung;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_HuTieu_HanhPhi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhPhi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhPhi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.HuTieu_HanhPhi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.HuTieu_HanhPhi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_HuTieu_GiaDo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GiaDo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GiaDo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.HuTieu_GiaDo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.HuTieu_GiaDo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_SupCua_ThitCua(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.CuaThit && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.CuaThit && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.SupCua_ThitCua;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.SupCua_ThitCua;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_SupCua_NamDongCo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NamDongCo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NamDongCo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.SupCua_NamDongCo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.SupCua_NamDongCo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_SupCua_HatBap(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Bap && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Bap && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.SupCua_HatBap;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.SupCua_HatBap;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_SupCua_BotNang(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotNang && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotNang && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.SupCua_BotNang;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.SupCua_BotNang;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_SupCua_TrungGa(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TrungGa && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TrungGa && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.SupCua_TrungGa;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.SupCua_TrungGa;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_XucXichChien_XucXich(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.XucXich && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.XucXich && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.XucXichChien_XucXich;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.XucXichChien_XucXich;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_XucXichChien_DauAn(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.XucXichChien_DauAn;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.XucXichChien_DauAn;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_XucXichChien_TuongOt(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TuongOt && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TuongOt && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.XucXichChien_TuongOt;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.XucXichChien_TuongOt;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_KhoaiTayChien_KhaoiTay(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.KhoaiTay && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.KhoaiTay && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.KhoaiTayChien_KhaoiTay;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.KhoaiTayChien_KhaoiTay;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_KhoaiTayChien_DauAn(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.KhoaiTayChien_DauAn;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.KhoaiTayChien_DauAn;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_KhoaiTayChien_TuongOt(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TuongOt && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TuongOt && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.KhoaiTayChien_TuongOt;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.KhoaiTayChien_TuongOt;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_BapRangBo_Bap(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Bap && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Bap && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BapRangBo_Bap;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BapRangBo_Bap;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BapRangBo_Bo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Bow && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Bow && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BapRangBo_Bo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BapRangBo_Bo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BapRangBo_Duong(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Duong && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Duong && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BapRangBo_Duong;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BapRangBo_Duong;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_PhoMaiQue_PhomaiMozzaarella(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.PhoMaiMozzarella && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.PhoMaiMozzarella && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PhoMaiQue_PhomaiMozzaarella;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PhoMaiQue_PhomaiMozzaarella;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_PhoMaiQue_BotChienXu(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotChienXu && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotChienXu && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PhoMaiQue_BotChienXu;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PhoMaiQue_BotChienXu;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_PhoMaiQue_DauAn(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.PhoMaiQue_DauAn;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.PhoMaiQue_DauAn;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_XienBan_XucXich(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.XucXich && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.XucXich && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.XienBan_XucXich;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.XienBan_XucXich;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_XienBan_XienBan(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.XienBan && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.XienBan && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.XienBan_XienBan;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.XienBan_XienBan;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_XienBan_NuocMam(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocMam && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocMam && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.XienBan_NuocMam;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.XienBan_NuocMam;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_XienBan_Duong(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Duong && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Duong && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.XienBan_Duong;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.XienBan_Duong;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_XienBan_ToiOt(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ToiOt && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ToiOt && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.XienBan_ToiOt;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.XienBan_ToiOt;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_XienBan_Cocacola(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Cocacola && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Cocacola && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.XienBan_Cocacola;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.XienBan_Cocacola;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_ComGaXoiMo_Gao(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Gao && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Gao && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComGaXoiMo_Gao;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComGaXoiMo_Gao;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ComGaXoiMo_Ga(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Ga && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Ga && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComGaXoiMo_Ga;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComGaXoiMo_Ga;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ComGaXoiMo_DuaLeo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DuaLeo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DuaLeo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComGaXoiMo_DuaLeo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComGaXoiMo_DuaLeo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ComGaXoiMo_RauSong(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauSong && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauSong && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComGaXoiMo_RauSong;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComGaXoiMo_RauSong;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ComGaXoiMo_NuocMam(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocMam && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocMam && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComGaXoiMo_NuocMam;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComGaXoiMo_NuocMam;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ComGaXoiMo_ToiOt(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ToiOt && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ToiOt && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComGaXoiMo_ToiOt;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComGaXoiMo_ToiOt;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_MiXaoOcMongTay_MiTrung(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MiTrung && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MiTrung && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MiXaoOcMongTay_MiTrung;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MiXaoOcMongTay_MiTrung;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_MiXaoOcMongTay_OcMongtay(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.OcMongTay && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.OcMongTay && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MiXaoOcMongTay_OcMongtay;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MiXaoOcMongTay_OcMongtay;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_MiXaoOcMongTay_RauRam(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauRam && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauRam && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MiXaoOcMongTay_RauRam;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MiXaoOcMongTay_RauRam;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_MiXaoOcMongTay_Toi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Toi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Toi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MiXaoOcMongTay_Toi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MiXaoOcMongTay_Toi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_MiXaoOcMongTay_DauHao(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauHao && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauHao && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MiXaoOcMongTay_DauHao;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MiXaoOcMongTay_DauHao;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_MiXaoOcMongTay_XiDau(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.XiDau && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.XiDau && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MiXaoOcMongTay_XiDau;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MiXaoOcMongTay_XiDau;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_ComboBanhNgot_BongLan(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhBongLan && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhBongLan && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComboBanhNgot_BongLan;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComboBanhNgot_BongLan;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ComboBanhNgot_XuKem(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhXuKem && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhXuKem && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComboBanhNgot_XuKem;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComboBanhNgot_XuKem;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ComboBanhNgot_Croissant(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhCroissant && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhCroissant && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ComboBanhNgot_Croissant;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ComboBanhNgot_Croissant;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_BanhCuon_BotBanhCuon(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotBanhCuon && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotBanhCuon && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhCuon_BotBanhCuon;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhCuon_BotBanhCuon;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhCuon_ThitBam(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBam && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBam && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhCuon_ThitBam;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhCuon_ThitBam;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhCuon_MocNhi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MocNhi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MocNhi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhCuon_MocNhi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhCuon_MocNhi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhCuon_HanhPhi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhPhi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhPhi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhCuon_HanhPhi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhCuon_HanhPhi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BanhCuon_NuocMam(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocMam && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NuocMam && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BanhCuon_NuocMam;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BanhCuon_NuocMam;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_GaComLam_GaNguyenCon(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GaNguyenCon && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GaNguyenCon && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GaComLam_GaNguyenCon;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GaComLam_GaNguyenCon;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GaComLam_GaoTe(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GaoTe && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.GaoTe && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GaComLam_GaoTe;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GaComLam_GaoTe;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GaComLam_BanhBaoChien(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhBaoChien && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhBaoChien && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GaComLam_BanhBaoChien;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GaComLam_BanhBaoChien;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GaComLam_RauSong(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauSong && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.RauSong && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GaComLam_RauSong;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GaComLam_RauSong;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_GaComLam_DuaLeo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DuaLeo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DuaLeo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.GaComLam_DuaLeo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.GaComLam_DuaLeo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_ChaGio_BanhTrang(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhTrang && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BanhTrang && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaGio_BanhTrang;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaGio_BanhTrang;

           _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChaGio_ThitBam(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBam && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBam && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaGio_ThitBam;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaGio_ThitBam;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChaGio_CaRot(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.CaRot && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.CaRot && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaGio_CaRot;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaGio_CaRot;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChaGio_Mien(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Mien && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Mien && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaGio_Mien;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaGio_Mien;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChaGio_NamMeo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NamMeo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.NamMeo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaGio_NamMeo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaGio_NamMeo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChaGio_HanhTim(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhTim && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhTim && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaGio_HanhTim;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaGio_HanhTim;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_ChaGio_TrungGa(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TrungGa && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TrungGa && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.ChaGio_TrungGa;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.ChaGio_TrungGa;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> UpdateQuantityMaterial_MucChien_MucTuoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MucTuoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MucTuoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MucChien_MucTuoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MucChien_MucTuoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_MucChien_BotChienGion(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotChienGion && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotChienGion && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MucChien_BotChienGion;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MucChien_BotChienGion;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_MucChien_TrungGa(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TrungGa && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TrungGa && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MucChien_TrungGa;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MucChien_TrungGa;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_MucChien_DauAn(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.MucChien_DauAn;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.MucChien_DauAn;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_TomChien_TomTuoi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TomTuoi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TomTuoi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TomChien_TomTuoi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TomChien_TomTuoi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_TomChien_BotChienXu(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotChienXu && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.BotChienXu && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TomChien_BotChienXu;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TomChien_BotChienXu;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_TomChien_TrungGa(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TrungGa && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.TrungGa && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TomChien_TrungGa;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TomChien_TrungGa;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_TomChien_DauAn(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.TomChien_DauAn;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.TomChien_DauAn;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_NacHeoXao_ThitHeo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBaChi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBaChi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.NacHeoXao_ThitHeo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.NacHeoXao_ThitHeo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_NacHeoXao_HanhTay(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhTya && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhTya && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.NacHeoXao_HanhTay;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.NacHeoXao_HanhTay;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_NacHeoXao_Toi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Toi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Toi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.NacHeoXao_Toi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.NacHeoXao_Toi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_NacHeoXao_DauHao(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauHao && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauHao && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.NacHeoXao_DauHao;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.NacHeoXao_DauHao;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_NacHeoXao_DauAn(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.NacHeoXao_DauAn;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.NacHeoXao_DauAn;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_RauXao_CaiNgot(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.CaiNgot && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.CaiNgot && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.RauXao_CaiNgot;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.RauXao_CaiNgot;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_RauXao_Toi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Toi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Toi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.RauXao_Toi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.RauXao_Toi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_RauXao_DauAn(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.RauXao_DauAn;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.RauXao_DauAn;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_RauXao_MuoiTieu(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MuoiTieu && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.MuoiTieu && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.RauXao_MuoiTieu;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.RauXao_MuoiTieu;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_RauXao_DauHao(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauHao && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauHao && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.RauXao_DauHao;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.RauXao_DauHao;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityMaterial_BoXao_ThitBo(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBo && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.ThitBo && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BoXao_ThitBo;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BoXao_ThitBo;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BoXao_HanhTay(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhTya && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.HanhTya && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BoXao_HanhTay;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BoXao_HanhTay;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BoXao_Toi(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Toi && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.Toi && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BoXao_Toi;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BoXao_Toi;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BoXao_DauHao(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauHao && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauHao && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BoXao_DauHao;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BoXao_DauHao;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateQuantityMaterial_BoXao_DauAn(int quantity)
        {
            var material = _appDbContext.Materials.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Quantity > 1);
            var warehouseHistory = _appDbContext.WarehouseHistorys.FirstOrDefault(item => item.MaterialId == _materialIdConfigs.DauAn && item.Using == 1 && item.CurrentQuantity > 1);
            if (material == null || warehouseHistory == null)
            {
                return false;
            }
            material.Quantity -= quantity * _materialConfigs.BoXao_DauAn;
            warehouseHistory.CurrentQuantity -= quantity * _materialConfigs.BoXao_DauAn;

            _appDbContext.Materials.Update(material);
            _appDbContext.WarehouseHistorys.Update(warehouseHistory);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
 