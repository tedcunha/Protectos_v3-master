﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Protectos.Application.ViewModels.Fornecedores
{
    public class FornecedorViewModel
    {
        public FornecedorViewModel()
        {
            Id = Guid.NewGuid();
            Enderecos = new List<FornecedorEnderecoViewModel>();
            Emails = new List<FornecedorEmailViewModel>();
            Telefones = new List<FornecedorTelefoneViewModel>();
            Ativo = true;
            DataCadastro = DateTime.Now;
            CadastradoPor = Guid.NewGuid();
            DataAlteracao = DateTime.Now;
            AlteradoPor = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; set; }
        [ScaffoldColumn(false)]
        public bool Ativo { get; set; }
        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }
        [ScaffoldColumn(false)]
        public Guid CadastradoPor { get; set; }
        [ScaffoldColumn(false)]
        public DateTime DataAlteracao { get; set; }
        [ScaffoldColumn(false)]
        public Guid AlteradoPor { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(100, ErrorMessage = "Máximo {0} caracteres")]
        [MinLength(2, ErrorMessage = "Mínimo {0} caracteres")]
        [DisplayName("Razão Social")]
        public string RazaoSocial { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(100, ErrorMessage = "Máximo {0} caracteres")]
        [MinLength(2, ErrorMessage = "Mínimo {0} caracteres")]
        [DisplayName("Nome Fantasia")]
        public string NomeFantasia { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(14, ErrorMessage = "Máximo {0} caracteres")]
        [MinLength(14, ErrorMessage = "Mínimo {0} caracteres")]
        [DisplayName("CNPJ")]
        public string Cnpj { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(14, ErrorMessage = "Máximo {0} caracteres")]
        [DisplayName("Inscrição Estadual")]
        public string InscricaoEstadual { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(14, ErrorMessage = "Máximo {0} caracteres")]
        [DisplayName("Inscrição Municipal")]
        public string InscricaoMunicipal { get; set; }
        [MaxLength(50, ErrorMessage = "Máximo {0} caracteres")]
        [DisplayName("Site")]
        public string Site { get; set; }
        public virtual IEnumerable<FornecedorEnderecoViewModel> Enderecos { get; set; }
        public virtual IEnumerable<FornecedorTelefoneViewModel> Telefones { get; set; }
        public virtual IEnumerable<FornecedorEmailViewModel> Emails { get; set; }
        [ScaffoldColumn(false)]
        public FluentValidation.Results.ValidationResult ValidationResult { get; set; }
    }
}