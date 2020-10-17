import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ProductTypes } from '@app/domain/enums';
import { Product } from '@app/domain/product';
import { Recipe } from '@app/domain/recipe';
import { RecipeDetail } from '@app/domain/recipe-detail';
import { DECIMAL_REGEX, NUMBER_REGEX } from '@app/helpers/validations/common-regex';
import { ProductService } from '@app/services/product.service';
import { RecipeService } from '@app/services/recipe.service';
import { faClock, faDollarSign, faEdit, faPlus, faSave, faTimes, faTrashAlt, faUpload, faUndo } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-recipes',
  templateUrl: './recipes.component.html',
  styleUrls: ['./recipes.component.css']
})
export class RecipesComponent implements OnInit {
  currentRecipe: Recipe = new Recipe();
  recipes: Recipe[] = [];
  products: Product[] = [];

  icons: any = {
    faSave: faSave,
    faTimes: faTimes,
    faEdit: faEdit,
    faTrashAlt: faTrashAlt,
    faPlus: faPlus,
    faUpload: faUpload,
    faClock: faClock,
    faDollarSign: faDollarSign,
    faUndo: faUndo
  };

  paginationConfig: any = {
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: this.recipes.length
  };

  selectedFile: any;

  loading: boolean = false;
  numberPattern = NUMBER_REGEX;
  decimalPattern = DECIMAL_REGEX;

  constructor(
    public recipeService: RecipeService,
    public productService: ProductService,
    private toastrService: ToastrService
  ) { };

  ngOnInit() {
    this.getRecipes();
    this.getProducts();
  };

  getRecipes() {
    this.loading = true;
    this.recipeService.getAll().subscribe((recipes: Recipe[]) => {
      this.recipes = recipes;
      this.loading = false;
    }, (error) => {
      this.loading = false;
    });
  };

  getProducts() {
    this.loading = true;

    let filters = {
      Active: true,
      ProductTypeId: ProductTypes.Supply
    }

    this.productService.filterBy(filters).subscribe((products: Product[]) => {
      this.products = products;
      this.loading = false;
    }, (error) => {
      this.loading = false;
    });
  };

  save(form: NgForm) {
    this.loading = true;
    if (form.valid) {
      if (!this.currentRecipe.Id) {
        // post
        this.recipeService
          .post(this.currentRecipe)
          .subscribe((createdRecipe: Recipe) => {
            this.currentRecipe = createdRecipe;
            this.recipes.push(createdRecipe);
            this.loading = false;
            this.toastrService.success('Se ha creado correctamente', 'Receta Creada');
            form.form.markAsPristine();
          }, (error) => {
            this.loading = false;
            let message = 'Error al crear la receta';
            if (error.status === 400) {
              message = error.error;
            }
            this.toastrService.error(message, 'Error');
          });
      } else {
        // put
        this.recipeService
          .put(this.currentRecipe.Id, this.currentRecipe)
          .subscribe((editedRecipe: Recipe) => {
            let cIndex = this.recipes.findIndex((c) => c.Id === editedRecipe.Id);
            this.recipes.splice(cIndex, 1, editedRecipe);
            this.loading = false;
            this.toastrService.success('Se ha editado correctamente', 'Receta Editada');
            form.form.markAsPristine();
          }, (error) => {
            this.loading = false;
            let message = 'Error al editar la receta';
            if (error.status === 400) {
              message = error.error;
            }
            this.toastrService.error(message, 'Error');
          });
      }
    }
  };

  edit(recipe: Recipe, form: NgForm) {
    this.currentRecipe = recipe;
    form.form.markAsPristine();
  };

  delete(recipe: Recipe) {
    this.loading = true;
    this.recipeService.delete(recipe.Id).subscribe((deleted: boolean) => {
      let cIndex = this.recipes.findIndex((c) => c.Id === recipe.Id);
      this.recipes.splice(cIndex, 1);
      this.toastrService.success('Se ha eliminado correctamente', 'Receta Eliminada');
      this.loading = false;
    }, (error) => {
      this.loading = false;
      let message = 'Error al eliminar la receta';
      if (error.status === 400) {
        message = error.error;
      }
      this.toastrService.error(message, 'Error');
    });
  };

  cancel() {
    this.currentRecipe = new Recipe();
  };

  deleteRecipeDetail(recipeDetail: RecipeDetail, form: NgForm) {
    if (!recipeDetail)
      return;

    if (recipeDetail.RecipeId) {
      recipeDetail.Active = false;
    }
    else {
      let index = this.currentRecipe.RecipeDetails.findIndex((c) => c.ProductId === recipeDetail.ProductId);
      this.currentRecipe.RecipeDetails.splice(index, 1);
    }
  };

  undoDeleteRecipeDetail(recipeDetail: RecipeDetail) {
    if (!recipeDetail)
      return;

    recipeDetail.Active = true;
  }

  onFileChanged(event) {
    if (!event || !event.target || !event.target.files || event.target.files.length === 0)
      return;

    let reader = new FileReader();

    this.loading = true;

    reader.onload = () => {
      this.currentRecipe.ImageBase64 = reader.result.toString();
      this.loading = false;
      console.log('onload: ', this.currentRecipe);
    };

    reader.onerror = () => {
      this.loading = false;
      this.selectedFile = null;
      this.toastrService.error('Ocurrió un error al leer el archivo', 'Error');
    };

    reader.readAsDataURL(event.target.files[0]);
  };

  getFileName(selectedFile) {
    if (selectedFile)
      return selectedFile.replace(/^.*[\\\/]/, '');

    return null;
  };

  getSelectedProductUnitOfMeasurement(productId) {
    if (!productId || !this.products || this.products.length === 0)
      return;

    let product = this.products.find(product => product.Id === productId);
    if (product)
      return product.MeasurementUnit.Code;
  };

  addRecipeDetail() {
    if (!this.currentRecipe)
      return;

    let defaultRecipeDetail = new RecipeDetail();
    if (!this.currentRecipe.RecipeDetails.find(x => x.RecipeId === defaultRecipeDetail.RecipeId && x.ProductId === defaultRecipeDetail.ProductId && x.Count === defaultRecipeDetail.Count))
      this.currentRecipe.RecipeDetails.push(defaultRecipeDetail);
  };

  getFormControlName(text, index) {
    return `${text}${index}`;
  };

  getOnlyUnselectedProducts(recipeDetail: RecipeDetail) {
    let unselectedProducts = this.products.filter(product => recipeDetail.ProductId === product.Id || !this.currentRecipe.RecipeDetails.find(rd => rd.ProductId === product.Id));
    // console.log('getOnlyUnselectedProducts: ', unselectedProducts);
    return unselectedProducts;
  };

  pageChanged(event) {
    this.paginationConfig.currentPage = event;
  };
}
