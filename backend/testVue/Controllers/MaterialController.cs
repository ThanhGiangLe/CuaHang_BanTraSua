using Microsoft.AspNetCore.Mvc;
using testVue.Datas;
using testVue.Handls;
using testVue.Models;
using testVue.ModelsRequest;

namespace testVue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private IHandleOrderItem _iHandleOrderItem;
        public MaterialController(AppDbContext appDbContext, IHandleOrderItem iHandleOrderItem)
        {
            _appDbContext = appDbContext;
            _iHandleOrderItem = iHandleOrderItem;
        }

        [HttpPost("update-quantity-materials-after-order")]
        public async Task<IActionResult> UpdateQuantityMaterialUpdateAfterOrder([FromBody] ListFoodOrderRequest request)
        {
            try
            {
                foreach (var item in request.Items)
                {
                    var _ = item.FoodItemId switch
                    {
                        1 => await _iHandleOrderItem.UpdateQuantityMaterialId_ComGa(item.Quantity),
                        2 => await _iHandleOrderItem.UpdateQuantityMaterialId_PhoBo(item.Quantity),
                        3 => await _iHandleOrderItem.UpdateQuantityMaterialId_PizzaPhoMai(item.Quantity),
                        4 => await _iHandleOrderItem.UpdateQuantityMaterialId_KemTraiCay(item.Quantity),
                        5 => await _iHandleOrderItem.UpdateQuantityMaterialId_TraSua(item.Quantity),
                        6 => await _iHandleOrderItem.UpdateQuantityMaterialId_Sandwich(item.Quantity),
                        7 => await _iHandleOrderItem.UpdateQuantityMaterialId_GaRan(item.Quantity),
                        8 => await _iHandleOrderItem.UpdateQuantityMaterialId_BanhXeo(item.Quantity),
                        9 => await _iHandleOrderItem.UpdateQuantityMaterialId_ChaoGa(item.Quantity),
                        10 => await _iHandleOrderItem.UpdateQuantityMaterialId_SinhToXoai(item.Quantity),
                        11 => await _iHandleOrderItem.UpdateQuantityMaterialId_MiXaoBo(item.Quantity),
                        12 => await _iHandleOrderItem.UpdateQuantityMaterialId_GoiCuon(item.Quantity),
                        13 => await _iHandleOrderItem.UpdateQuantityMaterialId_TraChanh(item.Quantity),
                        14 => await _iHandleOrderItem.UpdateQuantityMaterialId_KemDua(item.Quantity),
                        15 => await _iHandleOrderItem.UpdateQuantityMaterialId_BanhMiTrung(item.Quantity),
                        16 => await _iHandleOrderItem.UpdateQuantityMaterialId_HuTieuNamVang(item.Quantity),
                        17 => await _iHandleOrderItem.UpdateQuantityMaterialId_SupCua(item.Quantity),
                        18 => await _iHandleOrderItem.UpdateQuantityMaterialId_XucXichChien(item.Quantity),
                        19 => await _iHandleOrderItem.UpdateQuantityMaterialId_KhoaiTayChien(item.Quantity),
                        20 => await _iHandleOrderItem.UpdateQuantityMaterialId_BapRangBo(item.Quantity),
                        21 => await _iHandleOrderItem.UpdateQuantityMaterialId_PhoMaiQue(item.Quantity),
                        1002 => await _iHandleOrderItem.UpdateQuantityMaterialId_Combo_CaVienChien_Cocacola(item.Quantity),
                        1003 => await _iHandleOrderItem.UpdateQuantityMaterialId_ComGaXoiMo(item.Quantity),
                        1004 => await _iHandleOrderItem.UpdateQuantityMaterialId_MiXaoOcMongTay(item.Quantity),
                        1009 => await _iHandleOrderItem.UpdateQuantityMaterialId_Combo_BanhNgot(item.Quantity),
                        1010 => await _iHandleOrderItem.UpdateQuantityMaterialId_BanhCuon(item.Quantity),
                        1011 => await _iHandleOrderItem.UpdateQuantityMaterialId_GaNuongComLam_BanhBaoChien(item.Quantity),
                        _ => throw new ArgumentOutOfRangeException($"Unknown FoodItemId: {item.FoodItemId}")
                    };

                    foreach (var i in item.ListAdditionalFood)
                    {
                        var __ = i.Id switch
                        {
                            1 => await _iHandleOrderItem.UpdateQuantityMaterialId_TranChauDen_MonThem(i.Quantity),
                            2 => await _iHandleOrderItem.UpdateQuantityMaterialId_TranChauTrang_MonThem(i.Quantity),
                            3 => await _iHandleOrderItem.UpdateQuantityMaterialId_ThachCaPhe_MonThem(i.Quantity),
                            4 => await _iHandleOrderItem.UpdateQuantityMaterialId_PuddingTrung_MonThem(i.Quantity),
                            5 => await _iHandleOrderItem.UpdateQuantityMaterialId_RauCauDua_MonThem(i.Quantity),
                            6 => await _iHandleOrderItem.UpdateQuantityMaterialId_BanhPhoThem_MonThem(i.Quantity),
                            7 => await _iHandleOrderItem.UpdateQuantityMaterialId_TrungTran_MonThem(i.Quantity),
                            8 => await _iHandleOrderItem.UpdateQuantityMaterialId_BoVienThem_MonThem(i.Quantity),
                            9 => await _iHandleOrderItem.UpdateQuantityMaterialId_GioHeo_MonThem(i.Quantity),
                            10 => await _iHandleOrderItem.UpdateQuantityMaterialId_HanhPhi_MonThem(i.Quantity),
                            11 => await _iHandleOrderItem.UpdateQuantityMaterialId_RauSong_MonThem(i.Quantity),
                            12 => await _iHandleOrderItem.UpdateQuantityMaterialId_ChaGio_MonThem(i.Quantity),
                            13 => await _iHandleOrderItem.UpdateQuantityMaterialId_MucChien_MonThem(i.Quantity),
                            14 => await _iHandleOrderItem.UpdateQuantityMaterialId_TomChien_MonThem(i.Quantity),
                            15 => await _iHandleOrderItem.UpdateQuantityMaterialId_NacHeoXao_MonThem(i.Quantity),
                            16 => await _iHandleOrderItem.UpdateQuantityMaterialId_RauXao_MonThem(i.Quantity),
                            17 => await _iHandleOrderItem.UpdateQuantityMaterialId_BoXao_MonThem(i.Quantity),
                            _ => throw new ArgumentOutOfRangeException($"Unknown Id: {i.Id}")
                        };
                    }
                }
                return Ok(new { message = "Update successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }

    }
}
